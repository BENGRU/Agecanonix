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
