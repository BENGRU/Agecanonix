using Agecanonix.Application.DTOs.Contact;
using MediatR;

namespace Agecanonix.Application.Features.Contacts.Queries;

public record GetAllContactsQuery : IRequest<IEnumerable<ContactDto>>;
public record GetContactByIdQuery(Guid Id) : IRequest<ContactDto?>;
public record GetContactsByResidentIdQuery(Guid ResidentId) : IRequest<IEnumerable<ContactDto>>;
