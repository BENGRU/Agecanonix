using Agecanonix.Application.DTOs.FacilityPublic;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.FacilityPublics.Commands;

public class UpdateFacilityPublicCommandHandler : IRequestHandler<UpdateFacilityPublicCommand, FacilityPublicDto>
{
    private readonly IRepository<FacilityPublic> _publicRepository;

    public UpdateFacilityPublicCommandHandler(IRepository<FacilityPublic> publicRepository)
    {
        _publicRepository = publicRepository;
    }

    public async Task<FacilityPublicDto> Handle(UpdateFacilityPublicCommand request, CancellationToken cancellationToken)
    {
        var entity = await _publicRepository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new KeyNotFoundException($"Facility public with ID {request.Id} not found");

        request.Dto.Adapt(entity);
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = "system";

        await _publicRepository.UpdateAsync(entity, cancellationToken);
        return entity.Adapt<FacilityPublicDto>();
    }
}
