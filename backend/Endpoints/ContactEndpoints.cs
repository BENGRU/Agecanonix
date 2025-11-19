using Agecanonix.Application.DTOs.Contact;
using Agecanonix.Application.Features.Contacts.Commands;
using Agecanonix.Application.Features.Contacts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Agecanonix.Api.Endpoints;

public static class ContactEndpoints
{
    public static void MapContactEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/contacts")
            .WithTags("Contacts")
            .WithOpenApi();

        group.MapGet("/", async (IMediator mediator) =>
        {
            var contacts = await mediator.Send(new GetAllContactsQuery());
            return Results.Ok(contacts);
        })
        .WithName("GetAllContacts")
        .Produces<IEnumerable<ContactDto>>(200);

        group.MapGet("/{id}", async (Guid id, IMediator mediator) =>
        {
            var contact = await mediator.Send(new GetContactByIdQuery(id));
            return contact is not null ? Results.Ok(contact) : Results.NotFound();
        })
        .WithName("GetContactById")
        .Produces<ContactDto>(200)
        .Produces(404);

        group.MapGet("/resident/{residentId}", async (Guid residentId, IMediator mediator) =>
        {
            var contacts = await mediator.Send(new GetContactsByResidentIdQuery(residentId));
            return Results.Ok(contacts);
        })
        .WithName("GetContactsByResidentId")
        .Produces<IEnumerable<ContactDto>>(200);

        group.MapPost("/", async ([FromBody] CreateContactDto dto, IMediator mediator) =>
        {
            var contact = await mediator.Send(new CreateContactCommand(dto));
            return Results.Created($"/api/contacts/{contact.Id}", contact);
        })
        .WithName("CreateContact")
        .Produces<ContactDto>(201)
        .Produces(400);

        group.MapPut("/{id}", async (Guid id, [FromBody] UpdateContactDto dto, IMediator mediator) =>
        {
            try
            {
                var contact = await mediator.Send(new UpdateContactCommand(id, dto));
                return Results.Ok(contact);
            }
            catch (KeyNotFoundException)
            {
                return Results.NotFound();
            }
        })
        .WithName("UpdateContact")
        .Produces<ContactDto>(200)
        .Produces(404);

        group.MapDelete("/{id}", async (Guid id, IMediator mediator) =>
        {
            var result = await mediator.Send(new DeleteContactCommand(id));
            return result ? Results.NoContent() : Results.NotFound();
        })
        .WithName("DeleteContact")
        .Produces(204)
        .Produces(404);
    }
}
