using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Queries;

public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, IEnumerable<FacilityDto>>
{
    private readonly IRepository<Facility> _repository;
    private readonly IMapper _mapper;

    public GetAllFacilitiesQueryHandler(IRepository<Facility> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FacilityDto>> Handle(GetAllFacilitiesQuery request, CancellationToken cancellationToken)
    {
        var facilities = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<FacilityDto>>(facilities);
    }
}
