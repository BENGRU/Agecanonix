using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public class CreateFacilityCommandHandler : IRequestHandler<CreateFacilityCommand, FacilityDto>
{
    private readonly IRepository<Facility> _repository;
    private readonly IRepository<ServiceType> _categoryRepository;
    private readonly IRepository<TargetPopulation> _publicRepository;

    public CreateFacilityCommandHandler(
        IRepository<Facility> repository,
        IRepository<ServiceType> categoryRepository,
        IRepository<TargetPopulation> publicRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<FacilityDto> Handle(CreateFacilityCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Dto.ServiceTypeId, cancellationToken)
                       ?? throw new ArgumentException($"Facility category with ID {request.Dto.ServiceTypeId} not found");

        var facilityPublic = await _publicRepository.GetByIdAsync(category.TargetPopulationId, cancellationToken)
                           ?? throw new ArgumentException($"Facility public with ID {category.TargetPopulationId} not found");

        var facility = request.Dto.Adapt<Facility>();
        facility.CreatedAt = DateTime.UtcNow;
        facility.CreatedBy = "system"; // TODO: Get from authenticated user

        var created = await _repository.AddAsync(facility, cancellationToken);
        var dto = created.Adapt<FacilityDto>();
        dto.ServiceTypeName = category.Name;
        dto.TargetPopulationId = facilityPublic.Id;
        dto.TargetPopulationName = facilityPublic.Name;

        return dto;
    }
}
