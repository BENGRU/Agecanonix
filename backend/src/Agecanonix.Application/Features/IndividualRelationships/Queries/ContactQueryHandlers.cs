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
        var relationships = await _repository.GetAllAsync();
        return relationships.Adapt<IEnumerable<IndividualRelationshipDto>>();
    }
}

public class GetIndividualRelationshipByIdQueryHandler : IRequestHandler<GetIndividualRelationshipByIdQuery, IndividualRelationshipDto?>
{
    private readonly IRepository<IndividualRelationship> _repository;

    public GetIndividualRelationshipByIdQueryHandler(IRepository<IndividualRelationship> repository)
    {
        _repository = repository;
    }

    public async Task<IndividualRelationshipDto?> Handle(GetIndividualRelationshipByIdQuery request, CancellationToken cancellationToken)
    {
        var relationship = await _repository.GetByIdAsync(request.Id);
        return relationship?.Adapt<IndividualRelationshipDto>();
    }
}

public class GetIndividualRelationshipsByIndividualIdQueryHandler : IRequestHandler<GetIndividualRelationshipsByIndividualIdQuery, IEnumerable<IndividualRelationshipDto>>
{
    private readonly IRepository<IndividualRelationship> _repository;

    public GetIndividualRelationshipsByIndividualIdQueryHandler(IRepository<IndividualRelationship> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<IndividualRelationshipDto>> Handle(GetIndividualRelationshipsByIndividualIdQuery request, CancellationToken cancellationToken)
    {
        var relationships = await _repository.FindAsync(r => r.SourceIndividualId == request.IndividualId);
        return relationships.OrderBy(r => r.Priority).Adapt<IEnumerable<IndividualRelationshipDto>>();
    }
}
