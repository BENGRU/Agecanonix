using Agecanonix.Application.DTOs.TargetPopulation;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.TargetPopulations.Queries;

public class GetTargetPopulationByIdQueryHandler : IRequestHandler<GetTargetPopulationByIdQuery, TargetPopulationDto?>
{
    private readonly IRepository<TargetPopulation> _publicRepository;

    public GetTargetPopulationByIdQueryHandler(IRepository<TargetPopulation> publicRepository)
    {
        _publicRepository = publicRepository;
    }

    public async Task<TargetPopulationDto?> Handle(GetTargetPopulationByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _publicRepository.GetByIdAsync(request.Id, cancellationToken);
        return entity?.Adapt<TargetPopulationDto>();
    }
}
