using Agecanonix.Application.DTOs.FacilityPublic;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.FacilityPublics.Commands;

public class CreateFacilityPublicCommandHandler : IRequestHandler<CreateFacilityPublicCommand, FacilityPublicDto>
{
    private readonly IRepository<FacilityPublic> _publicRepository;

    public CreateFacilityPublicCommandHandler(IRepository<FacilityPublic> publicRepository)
    {
        _publicRepository = publicRepository;
    }

    public async Task<FacilityPublicDto> Handle(CreateFacilityPublicCommand request, CancellationToken cancellationToken)
    {
        var entity = request.Dto.Adapt<FacilityPublic>();
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedBy = "system";

        var created = await _publicRepository.AddAsync(entity, cancellationToken);
        return created.Adapt<FacilityPublicDto>();
    }
}
