using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Queries;

public class GetFacilityByIdQueryHandler : IRequestHandler<GetFacilityByIdQuery, FacilityDto?>
{
    private readonly IRepository<Facility> _repository;
    private readonly IMapper _mapper;

    public GetFacilityByIdQueryHandler(IRepository<Facility> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FacilityDto?> Handle(GetFacilityByIdQuery request, CancellationToken cancellationToken)
    {
        var facility = await _repository.GetByIdAsync(request.Id);
        return facility == null ? null : _mapper.Map<FacilityDto>(facility);
    }
}
