using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Queries;

public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, IEnumerable<FacilityDto>>
{
    private readonly IRepository<Facility> _repository;

    public GetAllFacilitiesQueryHandler(IRepository<Facility> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<FacilityDto>> Handle(GetAllFacilitiesQuery request, CancellationToken cancellationToken)
    {
        var facilities = await _repository.GetAllAsync();
        return facilities.Adapt<IEnumerable<FacilityDto>>();
    }
}
