using Agecanonix.Application.DTOs.ServiceType;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.ServiceTypes.Commands;

public class CreateServiceTypeCommandHandler : IRequestHandler<CreateServiceTypeCommand, ServiceTypeDto>
{
    private readonly IRepository<ServiceType> _categoryRepository;
    private readonly IRepository<TargetPopulation> _publicRepository;

    public CreateServiceTypeCommandHandler(
        IRepository<ServiceType> categoryRepository,
        IRepository<TargetPopulation> publicRepository)
    {
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<ServiceTypeDto> Handle(CreateServiceTypeCommand request, CancellationToken cancellationToken)
    {
        var facilityPublic = await _publicRepository.GetByIdAsync(request.Dto.TargetPopulationId, cancellationToken)
                           ?? throw new ArgumentException($"Facility public with ID {request.Dto.TargetPopulationId} not found");

        var category = request.Dto.Adapt<ServiceType>();
        category.CreatedAt = DateTime.UtcNow;
        category.CreatedBy = "system";

        var created = await _categoryRepository.AddAsync(category, cancellationToken);
        var dto = created.Adapt<ServiceTypeDto>();
        dto.TargetPopulationName = facilityPublic.Name;

        return dto;
    }
}
