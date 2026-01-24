using Agecanonix.Application.DTOs.FacilityCategory;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.FacilityCategories.Commands;

public class UpdateFacilityCategoryCommandHandler : IRequestHandler<UpdateFacilityCategoryCommand, FacilityCategoryDto>
{
    private readonly IRepository<FacilityCategory> _categoryRepository;
    private readonly IRepository<FacilityPublic> _publicRepository;

    public UpdateFacilityCategoryCommandHandler(
        IRepository<FacilityCategory> categoryRepository,
        IRepository<FacilityPublic> publicRepository)
    {
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<FacilityCategoryDto> Handle(UpdateFacilityCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken)
                       ?? throw new KeyNotFoundException($"Facility category with ID {request.Id} not found");

        var facilityPublic = await _publicRepository.GetByIdAsync(request.Dto.FacilityPublicId, cancellationToken)
                           ?? throw new ArgumentException($"Facility public with ID {request.Dto.FacilityPublicId} not found");

        request.Dto.Adapt(category);
        category.UpdatedAt = DateTime.UtcNow;
        category.UpdatedBy = "system";

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        var dto = category.Adapt<FacilityCategoryDto>();
        dto.FacilityPublicName = facilityPublic.Name;

        return dto;
    }
}
