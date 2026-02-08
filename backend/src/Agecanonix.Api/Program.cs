using Agecanonix.Infrastructure.Data;
using Agecanonix.Application.Interfaces;
using Agecanonix.Infrastructure.Repositories;
using Agecanonix.Api.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using System.Reflection;
using Mapster;
using System.Collections;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/agecanonix-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container
builder.Services.AddEndpointsApiExplorer();

// Detect Codespaces URL
var codespaceUrl = Environment.GetEnvironmentVariable("CODESPACE_NAME");
var serverUrl = !string.IsNullOrEmpty(codespaceUrl)
    ? $"https://{codespaceUrl}-5175.app.github.dev"
    : "http://localhost:5175";

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        // Manipulation du document pour ajouter le serveur Codespaces
        var serverType = Type.GetType("Microsoft.OpenApi.OpenApiServer, Microsoft.OpenApi");
        if (serverType != null)
        {
            dynamic server = Activator.CreateInstance(serverType)!;
            server.Url = serverUrl;
            server.Description = "API Server";

            var listType = typeof(List<>).MakeGenericType(serverType);
            dynamic serversList = Activator.CreateInstance(listType)!;
            serversList.Add(server);
            document.Servers = serversList;
        }
        return Task.CompletedTask;
    });
});

// Add MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(Assembly.Load("Agecanonix.Application"));
});

// Add Mapster
builder.Services.AddMapster();

// Add Repositories and Unit of Work
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<Agecanonix.Application.Interfaces.IUnitOfWork, Agecanonix.Infrastructure.Units.UnitOfWork>();

// Add Services
builder.Services.AddScoped<Agecanonix.Application.Services.PriorityManagementService>();

// Configure InMemory database (for development/testing)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("AgecanonixDb"));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader()
              .WithExposedHeaders("*");
    });
});

// Configure JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "YourSuperSecretKeyThatMustBeLong2024!";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"] ?? "AgecanonixAPI",
        ValidAudience = jwtSettings["Audience"] ?? "AgecanonixClient",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Agecanonix API v2.0");
        options.RoutePrefix = "swagger";
        options.DocumentTitle = "Agecanonix API Documentation";
    });
}

// Enable CORS (must be before Authentication/Authorization)
app.UseCors();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Root endpoint - API information
app.MapGet("/", () => new
{
    name = "Agecanonix API",
    version = "2.0.0",
    description = "Administrative and billing management API for nursing homes (EHPAD)",
    architecture = "Clean Architecture with .NET 10",
    endpoints = new
    {
        swagger = "/swagger",
        openApiSpec = "/openapi/v1.json",
        facilities = "/api/facilities",
        facilityCategories = "/api/facility-categories",
        facilityPublics = "/api/facility-publics",
        individuals = "/api/individuals",
        contacts = "/api/contacts",
        stays = "/api/stays"
    },
    technologies = new[]
    {
        ".NET 10",
        "Entity Framework Core 10",
        "InMemory Database",
        "JWT Authentication",
        "Serilog"
    }
})
.WithName("Root")
.ExcludeFromDescription();

// Health check endpoint
app.MapGet("/health", () => new
{
    status = "healthy",
    timestamp = DateTime.UtcNow
})
.WithName("HealthCheck");

// Map API endpoints
app.MapFacilityEndpoints();
app.MapServiceTypeEndpoints();
app.MapTargetPopulationEndpoints();
app.MapIndividualEndpoints();
app.MapIndividualRelationshipEndpoints();

Log.Information("Agecanonix API started");
app.Run();
