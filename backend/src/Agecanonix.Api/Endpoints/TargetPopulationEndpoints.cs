using Agecanonix.Application.DTOs.TargetPopulation;
using Agecanonix.Application.Features.TargetPopulations.Commands;
using Agecanonix.Application.Features.TargetPopulations.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class TargetPopulationEndpoints
{
    public static void MapTargetPopulationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/facility-publics")
            .WithTags("TargetPopulations");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var publics = await mediator.Send(new GetAllTargetPopulationsQuery());
            return Results.Ok(publics);
        })
        .WithName("GetAllTargetPopulations")
        .Produces<IEnumerable<TargetPopulationDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var facilityPublic = await mediator.Send(new GetTargetPopulationByIdQuery(id));
            return facilityPublic is not null ? Results.Ok(facilityPublic) : Results.NotFound();
        })
        .WithName("GetTargetPopulationById")
        .Produces<TargetPopulationDto>(200)
        .Produces(404);

        group.MapPost("/", async ([FromBody] CreateTargetPopulationDto dto, IMediator mediator) =>
        {
            var facilityPublic = await mediator.Send(new CreateTargetPopulationCommand(dto));
            return Results.Created($"/api/facility-publics/{facilityPublic.Id}", facilityPublic);
        })
        .WithName("CreateTargetPopulation")
        .Produces<TargetPopulationDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateTargetPopulationDto dto, IMediator mediator) =>
        {
            try
            {
                var facilityPublic = await mediator.Send(new UpdateTargetPopulationCommand(id, dto));
                return Results.Ok(facilityPublic);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateTargetPopulation")
        .Produces<TargetPopulationDto>(200)
        .Produces(404);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            try
            {
                var deleted = await mediator.Send(new DeleteTargetPopulationCommand(id));
                return deleted ? Results.NoContent() : Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("DeleteTargetPopulation")
        .Produces(204)
        .Produces(404)
        .Produces(400);
    }
}
