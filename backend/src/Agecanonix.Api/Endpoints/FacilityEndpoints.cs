using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Features.Facilities.Commands;
using Agecanonix.Application.Features.Facilities.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class FacilityEndpoints
{
    public static void MapFacilityEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/facilities")
            .WithTags("Facilities");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var facilities = await mediator.Send(new GetAllFacilitiesQuery());
            return Results.Ok(facilities);
        })
        .WithName("GetAllFacilities")
        .Produces<IEnumerable<FacilityDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var facility = await mediator.Send(new GetFacilityByIdQuery(id));
            return facility is not null ? Results.Ok(facility) : Results.NotFound();
        })
        .WithName("GetFacilityById")
        .Produces<FacilityDto>(200)
        .Produces(404);

        group.MapPost("/", async ([FromBody] CreateFacilityDto dto, IMediator mediator) =>
        {
            try
            {
                var facility = await mediator.Send(new CreateFacilityCommand(dto));
                return Results.Created($"/api/facilities/{facility.Id}", facility);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { error = ex.Message });
            }
        })
        .WithName("CreateFacility")
        .Produces<FacilityDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateFacilityDto dto, IMediator mediator) =>
        {
            try
            {
                var facility = await mediator.Send(new UpdateFacilityCommand(id, dto));
                return Results.Ok(facility);
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
        .WithName("UpdateFacility")
        .Produces<FacilityDto>(200)
        .Produces(404)
        .Produces(400);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteFacilityCommand(id));
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteFacility")
        .Produces(204)
        .Produces(404);
    }
}
