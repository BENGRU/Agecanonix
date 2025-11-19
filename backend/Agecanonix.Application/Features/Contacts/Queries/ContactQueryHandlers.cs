using Agecanonix.Application.DTOs.Contact;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Contacts.Queries;

public class GetAllContactsQueryHandler : IRequestHandler<GetAllContactsQuery, IEnumerable<ContactDto>>
{
    private readonly IRepository<Contact> _repository;
    private readonly IMapper _mapper;

    public GetAllContactsQueryHandler(IRepository<Contact> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContactDto>> Handle(GetAllContactsQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ContactDto>>(contacts);
    }
}

public class GetContactByIdQueryHandler : IRequestHandler<GetContactByIdQuery, ContactDto?>
{
    private readonly IRepository<Contact> _repository;
    private readonly IMapper _mapper;

    public GetContactByIdQueryHandler(IRepository<Contact> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactDto?> Handle(GetContactByIdQuery request, CancellationToken cancellationToken)
    {
        var contact = await _repository.GetByIdAsync(request.Id);
        return contact == null ? null : _mapper.Map<ContactDto>(contact);
    }
}

public class GetContactsByResidentIdQueryHandler : IRequestHandler<GetContactsByResidentIdQuery, IEnumerable<ContactDto>>
{
    private readonly IRepository<Contact> _repository;
    private readonly IMapper _mapper;

    public GetContactsByResidentIdQueryHandler(IRepository<Contact> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ContactDto>> Handle(GetContactsByResidentIdQuery request, CancellationToken cancellationToken)
    {
        var contacts = await _repository.FindAsync(c => c.ResidentId == request.ResidentId);
        return _mapper.Map<IEnumerable<ContactDto>>(contacts.OrderBy(c => c.Priority));
    }
}
