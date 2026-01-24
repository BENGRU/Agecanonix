using Agecanonix.Application.DTOs.FacilityPublic;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.FacilityPublics.Queries;

public class GetFacilityPublicByIdQueryHandler : IRequestHandler<GetFacilityPublicByIdQuery, FacilityPublicDto?>
{
    private readonly IRepository<FacilityPublic> _publicRepository;

    public GetFacilityPublicByIdQueryHandler(IRepository<FacilityPublic> publicRepository)
    {
        _publicRepository = publicRepository;
    }

    public async Task<FacilityPublicDto?> Handle(GetFacilityPublicByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _publicRepository.GetByIdAsync(request.Id, cancellationToken);
        return entity?.Adapt<FacilityPublicDto>();
    }
}
