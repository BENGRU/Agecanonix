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
using Microsoft.OpenApi.Models;

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
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Agecanonix API",
        Version = "v2.0",
        Description = "API de gestion administrative pour EHPAD - Clean Architecture avec .NET 10",
        Contact = new OpenApiContact
        {
            Name = "Agecanonix Support",
            Email = "support@agecanonix.fr"
        }
    });
});

// Add MediatR
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(Assembly.Load("Agecanonix.Application"));
});

// Add AutoMapper
builder.Services.AddAutoMapper(Assembly.Load("Agecanonix.Application"));

// Add Repositories
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Configure PostgreSQL database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Host=localhost;Database=agecanonix;Username=postgres;Password=postgres";

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
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

// Enable CORS
app.UseCors("AllowAll");

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Agecanonix API v2.0");
        options.RoutePrefix = "swagger"; // URL : /swagger
        options.DocumentTitle = "Agecanonix API Documentation";
    });
}

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
        residents = "/api/residents",
        contacts = "/api/contacts",
        stays = "/api/stays"
    },
    technologies = new[] 
    { 
        ".NET 10", 
        "Entity Framework Core 10", 
        "PostgreSQL", 
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
app.MapResidentEndpoints();
app.MapContactEndpoints();
app.MapStayEndpoints();

Log.Information("Agecanonix API started");
app.Run();
