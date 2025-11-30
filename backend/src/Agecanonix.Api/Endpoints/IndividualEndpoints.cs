using Agecanonix.Application.DTOs.Individual;
using Agecanonix.Application.Features.Individuals.Commands;
using Agecanonix.Application.Features.Individuals.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class IndividualEndpoints
{
    public static void MapIndividualEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/individuals")
            .WithTags("Individuals");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var individuals = await mediator.Send(new GetAllIndividualsQuery());
            return Results.Ok(individuals);
        })
        .WithName("GetAllResidents")
        .Produces<IEnumerable<IndividualDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var resident = await mediator.Send(new GetIndividualByIdQuery(id));
            return resident is not null ? Results.Ok(resident) : Results.NotFound();
        })
        .WithName("GetResidentById")
        .Produces<IndividualDto>(200)
        .Produces(404);

        group.MapPost("/", async ([FromBody] CreateIndividualDto dto, IMediator mediator) =>
        {
            var resident = await mediator.Send(new CreateIndividualCommand(dto));
            return Results.Created($"/api/individuals/{resident.Id}", resident);
        })
        .WithName("CreateResident")
        .Produces<IndividualDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateIndividualDto dto, IMediator mediator) =>
        {
            try
            {
                var resident = await mediator.Send(new UpdateIndividualCommand(id, dto));
                return Results.Ok(resident);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateResident")
        .Produces<IndividualDto>(200)
        .Produces(404);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteIndividualCommand(id));
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteResident")
        .Produces(204)
        .Produces(404);
    }
}
