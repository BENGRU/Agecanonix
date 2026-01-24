using Agecanonix.Application.DTOs.TargetPopulation;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.TargetPopulations.Commands;

public class UpdateTargetPopulationCommandHandler : IRequestHandler<UpdateTargetPopulationCommand, TargetPopulationDto>
{
    private readonly IRepository<TargetPopulation> _publicRepository;

    public UpdateTargetPopulationCommandHandler(IRepository<TargetPopulation> publicRepository)
    {
        _publicRepository = publicRepository;
    }

    public async Task<TargetPopulationDto> Handle(UpdateTargetPopulationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _publicRepository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new KeyNotFoundException($"Facility public with ID {request.Id} not found");

        request.Dto.Adapt(entity);
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = "system";

        await _publicRepository.UpdateAsync(entity, cancellationToken);
        return entity.Adapt<TargetPopulationDto>();
    }
}
