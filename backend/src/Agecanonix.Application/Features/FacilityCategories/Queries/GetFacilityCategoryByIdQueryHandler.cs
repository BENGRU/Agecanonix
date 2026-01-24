using Agecanonix.Application.DTOs.FacilityCategory;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.FacilityCategories.Queries;

public class GetFacilityCategoryByIdQueryHandler : IRequestHandler<GetFacilityCategoryByIdQuery, FacilityCategoryDto?>
{
    private readonly IRepository<FacilityCategory> _categoryRepository;
    private readonly IRepository<FacilityPublic> _publicRepository;

    public GetFacilityCategoryByIdQueryHandler(
        IRepository<FacilityCategory> categoryRepository,
        IRepository<FacilityPublic> publicRepository)
    {
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<FacilityCategoryDto?> Handle(GetFacilityCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return null;

        var dto = category.Adapt<FacilityCategoryDto>();
        var facilityPublic = await _publicRepository.GetByIdAsync(category.FacilityPublicId, cancellationToken);
        dto.FacilityPublicName = facilityPublic?.Name ?? string.Empty;

        return dto;
    }
}
