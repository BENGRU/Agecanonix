using Agecanonix.Application.DTOs.IndividualRelationship;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Queries;

public class GetIndividualRelationshipByIdQueryHandler : IRequestHandler<GetIndividualRelationshipByIdQuery, IndividualRelationshipDto?>
{
    private readonly IRepository<IndividualRelationship> _repository;

    public GetIndividualRelationshipByIdQueryHandler(IRepository<IndividualRelationship> repository)
    {
        _repository = repository;
    }

    public async Task<IndividualRelationshipDto?> Handle(GetIndividualRelationshipByIdQuery request, CancellationToken cancellationToken)
    {
        var relationship = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return relationship?.Adapt<IndividualRelationshipDto>();
    }
}
