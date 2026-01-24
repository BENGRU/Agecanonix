using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public class CreateFacilityCommandHandler : IRequestHandler<CreateFacilityCommand, FacilityDto>
{
    private readonly IRepository<Facility> _repository;
    private readonly IRepository<FacilityCategory> _categoryRepository;
    private readonly IRepository<FacilityPublic> _publicRepository;

    public CreateFacilityCommandHandler(
        IRepository<Facility> repository,
        IRepository<FacilityCategory> categoryRepository,
        IRepository<FacilityPublic> publicRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<FacilityDto> Handle(CreateFacilityCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Dto.FacilityCategoryId, cancellationToken)
                       ?? throw new ArgumentException($"Facility category with ID {request.Dto.FacilityCategoryId} not found");

        var facilityPublic = await _publicRepository.GetByIdAsync(category.FacilityPublicId, cancellationToken)
                           ?? throw new ArgumentException($"Facility public with ID {category.FacilityPublicId} not found");

        var facility = request.Dto.Adapt<Facility>();
        facility.CreatedAt = DateTime.UtcNow;
        facility.CreatedBy = "system"; // TODO: Get from authenticated user

        var created = await _repository.AddAsync(facility, cancellationToken);
        var dto = created.Adapt<FacilityDto>();
        dto.FacilityCategoryName = category.Name;
        dto.FacilityPublicId = facilityPublic.Id;
        dto.FacilityPublicName = facilityPublic.Name;

        return dto;
    }
}
