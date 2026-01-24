using Agecanonix.Application.DTOs.TargetPopulation;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.TargetPopulations.Commands;

public class CreateTargetPopulationCommandHandler : IRequestHandler<CreateTargetPopulationCommand, TargetPopulationDto>
{
    private readonly IRepository<TargetPopulation> _publicRepository;

    public CreateTargetPopulationCommandHandler(IRepository<TargetPopulation> publicRepository)
    {
        _publicRepository = publicRepository;
    }

    public async Task<TargetPopulationDto> Handle(CreateTargetPopulationCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.Adapt<TargetPopulation>();
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedBy = "system";

        var created = await _publicRepository.AddAsync(entity, cancellationToken);
        return created.Adapt<TargetPopulationDto>();
    }
}
