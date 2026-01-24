using Agecanonix.Application.DTOs.FacilityPublic;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.FacilityPublics.Queries;

public class GetAllFacilityPublicsQueryHandler : IRequestHandler<GetAllFacilityPublicsQuery, IEnumerable<FacilityPublicDto>>
{
    private readonly IRepository<FacilityPublic> _publicRepository;

    public GetAllFacilityPublicsQueryHandler(IRepository<FacilityPublic> publicRepository)
    {
        _publicRepository = publicRepository;
    }

    public async Task<IEnumerable<FacilityPublicDto>> Handle(GetAllFacilityPublicsQuery request, CancellationToken cancellationToken)
    {
        var publics = await _publicRepository.GetAllAsync(cancellationToken);
        return publics.Adapt<IEnumerable<FacilityPublicDto>>();
    }
}
