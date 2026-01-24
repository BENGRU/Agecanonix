using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, FacilityDto>
{
    private readonly IRepository<Facility> _repository;
    private readonly IRepository<ServiceType> _categoryRepository;
    private readonly IRepository<TargetPopulation> _publicRepository;

    public UpdateFacilityCommandHandler(
        IRepository<Facility> repository,
        IRepository<ServiceType> categoryRepository,
        IRepository<TargetPopulation> publicRepository)
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

        var category = await _categoryRepository.GetByIdAsync(request.Dto.ServiceTypeId, cancellationToken)
                       ?? throw new ArgumentException($"Facility category with ID {request.Dto.ServiceTypeId} not found");

        var facilityPublic = await _publicRepository.GetByIdAsync(category.TargetPopulationId, cancellationToken)
                           ?? throw new ArgumentException($"Facility public with ID {category.TargetPopulationId} not found");

        request.Dto.Adapt(facility);
        facility.UpdatedAt = DateTime.UtcNow;
        facility.UpdatedBy = "system"; // TODO: Get from authenticated user

        await _repository.UpdateAsync(facility, cancellationToken);
        var dto = facility.Adapt<FacilityDto>();
        dto.ServiceTypeName = category.Name;
        dto.TargetPopulationId = facilityPublic.Id;
        dto.TargetPopulationName = facilityPublic.Name;

        return dto;
    }
}
