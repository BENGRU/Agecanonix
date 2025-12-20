using Agecanonix.Application.DTOs.IndividualRelationship;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Commands;

public class CreateIndividualRelationshipCommandHandler : IRequestHandler<CreateIndividualRelationshipCommand, IndividualRelationshipDto>
{
    private readonly IRepository<IndividualRelationship> _repository;

    public CreateIndividualRelationshipCommandHandler(IRepository<IndividualRelationship> repository)
    {
        _repository = repository;
    }

    public async Task<IndividualRelationshipDto> Handle(CreateIndividualRelationshipCommand request, CancellationToken cancellationToken)
    {
        var relationship = request.Dto.Adapt<IndividualRelationship>();
        relationship.Id = Guid.NewGuid();
        relationship.CreatedAt = DateTime.UtcNow;
        relationship.CreatedBy = "system";

        var created = await _repository.AddAsync(relationship, cancellationToken);
        return created.Adapt<IndividualRelationshipDto>();
    }
}

public class UpdateIndividualRelationshipCommandHandler : IRequestHandler<UpdateIndividualRelationshipCommand, IndividualRelationshipDto>
{
    private readonly IRepository<IndividualRelationship> _repository;

    public UpdateIndividualRelationshipCommandHandler(IRepository<IndividualRelationship> repository)
    {
        _repository = repository;
    }

    public async Task<IndividualRelationshipDto> Handle(UpdateIndividualRelationshipCommand request, CancellationToken cancellationToken)
    {
        var relationship = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (relationship == null)
            throw new KeyNotFoundException($"IndividualRelationship with ID {request.Id} not found");

        request.Dto.Adapt(relationship);
        relationship.UpdatedAt = DateTime.UtcNow;
        relationship.UpdatedBy = "system";

        await _repository.UpdateAsync(relationship, cancellationToken);
        return relationship.Adapt<IndividualRelationshipDto>();
    }
}

public class DeleteIndividualRelationshipCommandHandler : IRequestHandler<DeleteIndividualRelationshipCommand, bool>
{
    private readonly IRepository<IndividualRelationship> _repository;

    public DeleteIndividualRelationshipCommandHandler(IRepository<IndividualRelationship> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteIndividualRelationshipCommand request, CancellationToken cancellationToken)
    {
        var relationship = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (relationship == null)
            return false;

        await _repository.DeleteAsync(relationship, cancellationToken);
        return true;
    }
}
