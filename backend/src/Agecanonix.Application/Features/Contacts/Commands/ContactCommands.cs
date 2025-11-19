using Agecanonix.Application.DTOs.Contact;
using MediatR;

namespace Agecanonix.Application.Features.Contacts.Commands;

public record CreateContactCommand(CreateContactDto Dto) : IRequest<ContactDto>;
public record UpdateContactCommand(Guid Id, UpdateContactDto Dto) : IRequest<ContactDto>;
public record DeleteContactCommand(Guid Id) : IRequest<bool>;
