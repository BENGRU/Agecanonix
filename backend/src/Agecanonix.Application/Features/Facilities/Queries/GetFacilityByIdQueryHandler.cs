using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Queries;

public class GetFacilityByIdQueryHandler : IRequestHandler<GetFacilityByIdQuery, FacilityDto?>
{
    private readonly IRepository<Facility> _repository;
    private readonly IRepository<FacilityCategory> _categoryRepository;
    private readonly IRepository<FacilityPublic> _publicRepository;

    public GetFacilityByIdQueryHandler(
        IRepository<Facility> repository,
        IRepository<FacilityCategory> categoryRepository,
        IRepository<FacilityPublic> publicRepository)
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
        var category = await _categoryRepository.GetByIdAsync(facility.FacilityCategoryId, cancellationToken);
        dto.FacilityCategoryName = category?.Name ?? string.Empty;

        if (category is not null)
        {
            var facilityPublic = await _publicRepository.GetByIdAsync(category.FacilityPublicId, cancellationToken);
            dto.FacilityPublicId = category.FacilityPublicId;
            dto.FacilityPublicName = facilityPublic?.Name ?? string.Empty;
        }

        return dto;
    }
}
