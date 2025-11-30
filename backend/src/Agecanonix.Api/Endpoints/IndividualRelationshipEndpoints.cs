using Agecanonix.Application.DTOs.IndividualRelationship;
using Agecanonix.Application.Features.IndividualRelationships.Commands;
using Agecanonix.Application.Features.IndividualRelationships.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class IndividualRelationshipEndpoints
{
    public static void MapIndividualRelationshipEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/individual-relationships")
            .WithTags("IndividualRelationships");

        group.MapGet("/", async (IMediator mediator) =>
        {
            var relationships = await mediator.Send(new GetAllIndividualRelationshipsQuery());
            return Results.Ok(relationships);
        })
        .WithName("GetAllIndividualRelationships")
        .Produces<IEnumerable<IndividualRelationshipDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var relationship = await mediator.Send(new GetIndividualRelationshipByIdQuery(id));
            return relationship is not null ? Results.Ok(relationship) : Results.NotFound();
        })
        .WithName("GetIndividualRelationshipById")
        .Produces<IndividualRelationshipDto>(200)
        .Produces(404);

        group.MapGet("/individual/{individualId}", async (Guid individualId, IMediator mediator) =>
        {
            var relationships = await mediator.Send(new GetIndividualRelationshipsByIndividualIdQuery(individualId));
            return Results.Ok(relationships);
        })
        .WithName("GetIndividualRelationshipsByIndividualId")
        .Produces<IEnumerable<IndividualRelationshipDto>>(200);

        group.MapPost("/", async ([FromBody] CreateIndividualRelationshipDto dto, IMediator mediator) =>
        {
            var relationship = await mediator.Send(new CreateIndividualRelationshipCommand(dto));
            return Results.Created($"/api/contacts/{relationship.Id}", relationship);
        })
        .WithName("CreateContact")
        .Produces<IndividualRelationshipDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateIndividualRelationshipDto dto, IMediator mediator) =>
        {
            try
            {
                var relationship = await mediator.Send(new UpdateIndividualRelationshipCommand(id, dto));
                return Results.Ok(relationship);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateContact")
        .Produces<IndividualRelationshipDto>(200)
        .Produces(404);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteIndividualRelationshipCommand(id));
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteContact")
        .Produces(204)
        .Produces(404);
    }
}
