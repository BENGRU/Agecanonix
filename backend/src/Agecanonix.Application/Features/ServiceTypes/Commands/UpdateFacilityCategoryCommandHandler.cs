using Agecanonix.Application.DTOs.ServiceType;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.ServiceTypes.Commands;

public class UpdateServiceTypeCommandHandler : IRequestHandler<UpdateServiceTypeCommand, ServiceTypeDto>
{
    private readonly IRepository<ServiceType> _categoryRepository;
    private readonly IRepository<TargetPopulation> _publicRepository;

    public UpdateServiceTypeCommandHandler(
        IRepository<ServiceType> categoryRepository,
        IRepository<TargetPopulation> publicRepository)
    {
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<ServiceTypeDto> Handle(UpdateServiceTypeCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken)
                       ?? throw new KeyNotFoundException($"Facility category with ID {request.Id} not found");

        var facilityPublic = await _publicRepository.GetByIdAsync(request.Dto.TargetPopulationId, cancellationToken)
                           ?? throw new ArgumentException($"Facility public with ID {request.Dto.TargetPopulationId} not found");

        request.Dto.Adapt(category);
        category.UpdatedAt = DateTime.UtcNow;
        category.UpdatedBy = "system";

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        var dto = category.Adapt<ServiceTypeDto>();
        dto.TargetPopulationName = facilityPublic.Name;

        return dto;
    }
}
