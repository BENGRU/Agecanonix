using Agecanonix.Application.DTOs.Stay;
using Agecanonix.Application.Features.Stays.Commands;
using Agecanonix.Application.Features.Stays.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class StayEndpoints
{
    public static void MapStayEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/stays")
            .WithTags("Stays");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var stays = await mediator.Send(new GetAllStaysQuery());
            return Results.Ok(stays);
        })
        .WithName("GetAllStays")
        .Produces<IEnumerable<StayDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var stay = await mediator.Send(new GetStayByIdQuery(id));
            return stay is not null ? Results.Ok(stay) : Results.NotFound();
        })
        .WithName("GetStayById")
        .Produces<StayDto>(200)
        .Produces(404);

        group.MapGet("/individual/{individualId}", async (Guid individualId, IMediator mediator) =>
        {
            var stays = await mediator.Send(new GetStaysByIndividualIdQuery(individualId));
            return Results.Ok(stays);
        })
        .WithName("GetStaysByIndividualId")
        .Produces<IEnumerable<StayDto>>(200);

        group.MapGet("/facility/{facilityId}", async (Guid facilityId, IMediator mediator) =>
        {
            var stays = await mediator.Send(new GetStaysByFacilityIdQuery(facilityId));
            return Results.Ok(stays);
        })
        .WithName("GetStaysByFacilityId")
        .Produces<IEnumerable<StayDto>>(200);

        group.MapPost("/", async ([FromBody] CreateStayDto dto, IMediator mediator) =>
        {
            var stay = await mediator.Send(new CreateStayCommand(dto));
            return Results.Created($"/api/stays/{stay.Id}", stay);
        })
        .WithName("CreateStay")
        .Produces<StayDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateStayDto dto, IMediator mediator) =>
        {
            try
            {
                var stay = await mediator.Send(new UpdateStayCommand(id, dto));
                return Results.Ok(stay);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateStay")
        .Produces<StayDto>(200)
        .Produces(404);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteStayCommand(id));
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteStay")
        .Produces(204)
        .Produces(404);
    }
}
