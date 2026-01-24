using Agecanonix.Application.DTOs.TargetPopulation;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.TargetPopulations.Queries;

public class GetAllTargetPopulationsQueryHandler : IRequestHandler<GetAllTargetPopulationsQuery, IEnumerable<TargetPopulationDto>>
{
    private readonly IRepository<TargetPopulation> _publicRepository;

    public GetAllTargetPopulationsQueryHandler(IRepository<TargetPopulation> publicRepository)
    {
        _publicRepository = publicRepository;
    }

    public async Task<IEnumerable<TargetPopulationDto>> Handle(GetAllTargetPopulationsQuery request, CancellationToken cancellationToken)
    {
        var publics = await _publicRepository.GetAllAsync(cancellationToken);
        return publics.Adapt<IEnumerable<TargetPopulationDto>>();
    }
}
