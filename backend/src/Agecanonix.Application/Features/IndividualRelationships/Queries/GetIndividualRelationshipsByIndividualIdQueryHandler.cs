using Agecanonix.Application.DTOs.IndividualRelationship;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Queries;

public class GetIndividualRelationshipsByIndividualIdQueryHandler : IRequestHandler<GetIndividualRelationshipsByIndividualIdQuery, IEnumerable<IndividualRelationshipDto>>
{
    private readonly IRepository<IndividualRelationship> _repository;

    public GetIndividualRelationshipsByIndividualIdQueryHandler(IRepository<IndividualRelationship> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<IndividualRelationshipDto>> Handle(GetIndividualRelationshipsByIndividualIdQuery request, CancellationToken cancellationToken)
    {
        var relationships = await _repository.FindAsync(r => r.SourceIndividualId == request.IndividualId, cancellationToken);
        return relationships.OrderBy(r => r.Priority).Adapt<IEnumerable<IndividualRelationshipDto>>();
    }
}
