using Agecanonix.Application.DTOs.Resident;
using Agecanonix.Application.Features.Residents.Commands;
using Agecanonix.Application.Features.Residents.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class ResidentEndpoints
{
    public static void MapResidentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/residents")
            .WithTags("Residents");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var residents = await mediator.Send(new GetAllResidentsQuery());
            return Results.Ok(residents);
        })
        .WithName("GetAllResidents")
        .Produces<IEnumerable<ResidentDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var resident = await mediator.Send(new GetResidentByIdQuery(id));
            return resident is not null ? Results.Ok(resident) : Results.NotFound();
        })
        .WithName("GetResidentById")
        .Produces<ResidentDto>(200)
        .Produces(404);

        group.MapPost("/", async ([FromBody] CreateResidentDto dto, IMediator mediator) =>
        {
            var resident = await mediator.Send(new CreateResidentCommand(dto));
            return Results.Created($"/api/residents/{resident.Id}", resident);
        })
        .WithName("CreateResident")
        .Produces<ResidentDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateResidentDto dto, IMediator mediator) =>
        {
            try
            {
                var resident = await mediator.Send(new UpdateResidentCommand(id, dto));
                return Results.Ok(resident);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateResident")
        .Produces<ResidentDto>(200)
        .Produces(404);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteResidentCommand(id));
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteResident")
        .Produces(204)
        .Produces(404);
    }
}
