using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Queries;

public class GetFacilityByIdQueryHandler : IRequestHandler<GetFacilityByIdQuery, FacilityDto?>
{
    private readonly IRepository<Facility> _repository;
    private readonly IRepository<ServiceType> _categoryRepository;
    private readonly IRepository<TargetPopulation> _publicRepository;

    public GetFacilityByIdQueryHandler(
        IRepository<Facility> repository,
        IRepository<ServiceType> categoryRepository,
        IRepository<TargetPopulation> publicRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<FacilityDto?> Handle(GetFacilityByIdQuery request, CancellationToken cancellationToken)
    {
        var facility = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (facility is null)
            return null;

        var dto = facility.Adapt<FacilityDto>();
        var category = await _categoryRepository.GetByIdAsync(facility.ServiceTypeId, cancellationToken);
        dto.ServiceTypeName = category?.Name ?? string.Empty;

        if (category is not null)
        {
            var facilityPublic = await _publicRepository.GetByIdAsync(category.TargetPopulationId, cancellationToken);
            dto.TargetPopulationId = category.TargetPopulationId;
            dto.TargetPopulationName = facilityPublic?.Name ?? string.Empty;
        }

        return dto;
    }
}
