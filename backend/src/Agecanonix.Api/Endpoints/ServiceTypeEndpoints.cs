using Agecanonix.Application.DTOs.ServiceType;
using Agecanonix.Application.Features.ServiceTypes.Commands;
using Agecanonix.Application.Features.ServiceTypes.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class ServiceTypeEndpoints
{
    public static void MapServiceTypeEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/facility-categories")
            .WithTags("ServiceTypes");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var categories = await mediator.Send(new GetAllServiceTypesQuery());
            return Results.Ok(categories);
        })
        .WithName("GetAllServiceTypes")
        .Produces<IEnumerable<ServiceTypeDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var category = await mediator.Send(new GetServiceTypeByIdQuery(id));
            return category is not null ? Results.Ok(category) : Results.NotFound();
        })
        .WithName("GetServiceTypeById")
        .Produces<ServiceTypeDto>(200)
        .Produces(404);

        group.MapPost("/", async ([FromBody] CreateServiceTypeDto dto, IMediator mediator) =>
        {
            try
            {
                var category = await mediator.Send(new CreateServiceTypeCommand(dto));
                return Results.Created($"/api/facility-categories/{category.Id}", category);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("CreateServiceType")
        .Produces<ServiceTypeDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateServiceTypeDto dto, IMediator mediator) =>
        {
            try
            {
                var category = await mediator.Send(new UpdateServiceTypeCommand(id, dto));
                return Results.Ok(category);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("UpdateServiceType")
        .Produces<ServiceTypeDto>(200)
        .Produces(404)
        .Produces(400);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            try
            {
                var deleted = await mediator.Send(new DeleteServiceTypeCommand(id));
                return deleted ? Results.NoContent() : Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("DeleteServiceType")
        .Produces(204)
        .Produces(404)
        .Produces(400);
    }
}
