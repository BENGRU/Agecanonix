using Agecanonix.Application.DTOs.Contact;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Contacts.Commands;

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, ContactDto>
{
    private readonly IRepository<Contact> _repository;
    private readonly IMapper _mapper;

    public CreateContactCommandHandler(IRepository<Contact> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = _mapper.Map<Contact>(request.Dto);
        contact.Id = Guid.NewGuid();
        contact.CreatedAt = DateTime.UtcNow;
        contact.CreatedBy = "system";

        var created = await _repository.AddAsync(contact);
        return _mapper.Map<ContactDto>(created);
    }
}

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, ContactDto>
{
    private readonly IRepository<Contact> _repository;
    private readonly IMapper _mapper;

    public UpdateContactCommandHandler(IRepository<Contact> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ContactDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _repository.GetByIdAsync(request.Id);
        if (contact == null)
            throw new KeyNotFoundException($"Contact with ID {request.Id} not found");

        _mapper.Map(request.Dto, contact);
        contact.UpdatedAt = DateTime.UtcNow;
        contact.UpdatedBy = "system";

        await _repository.UpdateAsync(contact);
        return _mapper.Map<ContactDto>(contact);
    }
}

public class DeleteContactCommandHandler : IRequestHandler<DeleteContactCommand, bool>
{
    private readonly IRepository<Contact> _repository;

    public DeleteContactCommandHandler(IRepository<Contact> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _repository.GetByIdAsync(request.Id);
        if (contact == null)
            return false;

        await _repository.DeleteAsync(contact);
        return true;
    }
}
