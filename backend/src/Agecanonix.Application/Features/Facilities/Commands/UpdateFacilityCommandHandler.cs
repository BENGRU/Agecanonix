using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, FacilityDto>
{
    private readonly IRepository<Facility> _repository;
    private readonly IRepository<FacilityCategory> _categoryRepository;
    private readonly IRepository<FacilityPublic> _publicRepository;

    public UpdateFacilityCommandHandler(
        IRepository<Facility> repository,
        IRepository<FacilityCategory> categoryRepository,
        IRepository<FacilityPublic> publicRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<FacilityDto> Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
    {
        var facility = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (facility == null)
            throw new KeyNotFoundException($"Facility with ID {request.Id} not found");

        var category = await _categoryRepository.GetByIdAsync(request.Dto.FacilityCategoryId, cancellationToken)
                       ?? throw new ArgumentException($"Facility category with ID {request.Dto.FacilityCategoryId} not found");

        var facilityPublic = await _publicRepository.GetByIdAsync(category.FacilityPublicId, cancellationToken)
                           ?? throw new ArgumentException($"Facility public with ID {category.FacilityPublicId} not found");

        request.Dto.Adapt(facility);
        facility.UpdatedAt = DateTime.UtcNow;
        facility.UpdatedBy = "system"; // TODO: Get from authenticated user

        await _repository.UpdateAsync(facility, cancellationToken);
        var dto = facility.Adapt<FacilityDto>();
        dto.FacilityCategoryName = category.Name;
        dto.FacilityPublicId = facilityPublic.Id;
        dto.FacilityPublicName = facilityPublic.Name;

        return dto;
    }
}
