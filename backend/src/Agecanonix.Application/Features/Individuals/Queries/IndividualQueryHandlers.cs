using Agecanonix.Application.DTOs.Individual;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Individuals.Queries;

public class GetAllIndividualsQueryHandler : IRequestHandler<GetAllIndividualsQuery, IEnumerable<IndividualDto>>
{
    private readonly IRepository<Individual> _repository;

    public GetAllIndividualsQueryHandler(IRepository<Individual> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<IndividualDto>> Handle(GetAllIndividualsQuery request, CancellationToken cancellationToken)
    {
        var individuals = await _repository.GetAllAsync(cancellationToken);
        return individuals.Adapt<IEnumerable<IndividualDto>>();
    }
}

public class GetIndividualByIdQueryHandler : IRequestHandler<GetIndividualByIdQuery, IndividualDto?>
{
    private readonly IRepository<Individual> _repository;

    public GetIndividualByIdQueryHandler(IRepository<Individual> repository)
    {
        _repository = repository;
    }

    public async Task<IndividualDto?> Handle(GetIndividualByIdQuery request, CancellationToken cancellationToken)
    {
        var resident = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return resident?.Adapt<IndividualDto>();
    }
}
