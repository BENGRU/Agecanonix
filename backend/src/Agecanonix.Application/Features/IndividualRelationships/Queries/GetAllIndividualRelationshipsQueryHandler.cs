using Agecanonix.Application.DTOs.IndividualRelationship;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Queries;

public class GetAllIndividualRelationshipsQueryHandler : IRequestHandler<GetAllIndividualRelationshipsQuery, IEnumerable<IndividualRelationshipDto>>
{
    private readonly IRepository<IndividualRelationship> _repository;

    public GetAllIndividualRelationshipsQueryHandler(IRepository<IndividualRelationship> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<IndividualRelationshipDto>> Handle(GetAllIndividualRelationshipsQuery request, CancellationToken cancellationToken)
    {
        var relationships = await _repository.GetAllAsync(cancellationToken);
        return relationships.Adapt<IEnumerable<IndividualRelationshipDto>>();
    }
}
