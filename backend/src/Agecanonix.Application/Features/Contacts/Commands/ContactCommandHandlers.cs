using Agecanonix.Application.DTOs.Contact;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Contacts.Commands;

public class CreateContactCommandHandler : IRequestHandler<CreateContactCommand, ContactDto>
{
    private readonly IRepository<Contact> _repository;

    public CreateContactCommandHandler(IRepository<Contact> repository)
    {
        _repository = repository;
    }

    public async Task<ContactDto> Handle(CreateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = request.Dto.Adapt<Contact>();
        contact.Id = Guid.NewGuid();
        contact.CreatedAt = DateTime.UtcNow;
        contact.CreatedBy = "system";

        var created = await _repository.AddAsync(contact);
        return created.Adapt<ContactDto>();
    }
}

public class UpdateContactCommandHandler : IRequestHandler<UpdateContactCommand, ContactDto>
{
    private readonly IRepository<Contact> _repository;

    public UpdateContactCommandHandler(IRepository<Contact> repository)
    {
        _repository = repository;
    }

    public async Task<ContactDto> Handle(UpdateContactCommand request, CancellationToken cancellationToken)
    {
        var contact = await _repository.GetByIdAsync(request.Id);
        if (contact == null)
            throw new KeyNotFoundException($"Contact with ID {request.Id} not found");

        request.Dto.Adapt(contact);
        contact.UpdatedAt = DateTime.UtcNow;
        contact.UpdatedBy = "system";

        await _repository.UpdateAsync(contact);
        return contact.Adapt<ContactDto>();
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
