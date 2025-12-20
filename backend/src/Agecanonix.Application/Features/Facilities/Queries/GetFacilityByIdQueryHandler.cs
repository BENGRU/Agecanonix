using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Queries;

public class GetFacilityByIdQueryHandler : IRequestHandler<GetFacilityByIdQuery, FacilityDto?>
{
    private readonly IRepository<Facility> _repository;

    public GetFacilityByIdQueryHandler(IRepository<Facility> repository)
    {
        _repository = repository;
    }

    public async Task<FacilityDto?> Handle(GetFacilityByIdQuery request, CancellationToken cancellationToken)
    {
        var facility = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return facility?.Adapt<FacilityDto>();
    }
}
