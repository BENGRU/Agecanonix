using Agecanonix.Application.DTOs.FacilityPublic;
using Agecanonix.Application.Features.FacilityPublics.Commands;
using Agecanonix.Application.Features.FacilityPublics.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class FacilityPublicEndpoints
{
    public static void MapFacilityPublicEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/facility-publics")
            .WithTags("FacilityPublics");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var publics = await mediator.Send(new GetAllFacilityPublicsQuery());
            return Results.Ok(publics);
        })
        .WithName("GetAllFacilityPublics")
        .Produces<IEnumerable<FacilityPublicDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var facilityPublic = await mediator.Send(new GetFacilityPublicByIdQuery(id));
            return facilityPublic is not null ? Results.Ok(facilityPublic) : Results.NotFound();
        })
        .WithName("GetFacilityPublicById")
        .Produces<FacilityPublicDto>(200)
        .Produces(404);

        group.MapPost("/", async ([FromBody] CreateFacilityPublicDto dto, IMediator mediator) =>
        {
            var facilityPublic = await mediator.Send(new CreateFacilityPublicCommand(dto));
            return Results.Created($"/api/facility-publics/{facilityPublic.Id}", facilityPublic);
        })
        .WithName("CreateFacilityPublic")
        .Produces<FacilityPublicDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateFacilityPublicDto dto, IMediator mediator) =>
        {
            try
            {
                var facilityPublic = await mediator.Send(new UpdateFacilityPublicCommand(id, dto));
                return Results.Ok(facilityPublic);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateFacilityPublic")
        .Produces<FacilityPublicDto>(200)
        .Produces(404);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            try
            {
                var deleted = await mediator.Send(new DeleteFacilityPublicCommand(id));
                return deleted ? Results.NoContent() : Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("DeleteFacilityPublic")
        .Produces(204)
        .Produces(404)
        .Produces(400);
    }
}
