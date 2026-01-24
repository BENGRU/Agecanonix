using Agecanonix.Application.DTOs.FacilityCategory;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.FacilityCategories.Commands;

public class CreateFacilityCategoryCommandHandler : IRequestHandler<CreateFacilityCategoryCommand, FacilityCategoryDto>
{
    private readonly IRepository<FacilityCategory> _categoryRepository;
    private readonly IRepository<FacilityPublic> _publicRepository;

    public CreateFacilityCategoryCommandHandler(
        IRepository<FacilityCategory> categoryRepository,
        IRepository<FacilityPublic> publicRepository)
    {
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<FacilityCategoryDto> Handle(CreateFacilityCategoryCommand request, CancellationToken cancellationToken)
    {
        var facilityPublic = await _publicRepository.GetByIdAsync(request.Dto.FacilityPublicId, cancellationToken)
                           ?? throw new ArgumentException($"Facility public with ID {request.Dto.FacilityPublicId} not found");

        var category = request.Dto.Adapt<FacilityCategory>();
        category.CreatedAt = DateTime.UtcNow;
        category.CreatedBy = "system";

        var created = await _categoryRepository.AddAsync(category, cancellationToken);
        var dto = created.Adapt<FacilityCategoryDto>();
        dto.FacilityPublicName = facilityPublic.Name;

        return dto;
    }
}
