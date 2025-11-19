using Agecanonix.Application.DTOs.Contact;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Contacts.Queries;

public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactDto>>
{
    private readonly IRepository<Contact> _repository;

    public GetAllContactsQueryHandler(IRepository<Contact> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _repository.GetAllAsync();
        return contacts.Adapt<IEnumerable<ContactDto>>();
    }
}

public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto?>
{
    private readonly IRepository<Contact> _repository;

    public GetContactByIdQueryHandler(IRepository<Contact> repository)
    {
        _repository = repository;
    }

    public async Task<ContactDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        var contact = await _repository.GetByIdAsync(request.Id);
        return contact?.Adapt<ContactDto>();
    }
}

public class GetContactsByResidentIdQueryHandler : IRequestHandler<GetContactsByResidentIdQuery, IEnumerable<ContactDto>>
{
    private readonly IRepository<Contact> _repository;

    public GetContactsByResidentIdQueryHandler(IRepository<Contact> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ContactDto>> Handle(GetContactsByResidentIdQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _repository.FindAsync(c => c.ResidentId == request.ResidentId);
        return contacts.OrderBy(c => c.Priority).Adapt<IEnumerable<ContactDto>>();
    }
}
