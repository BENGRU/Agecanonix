using Agecanonix.Application.DTOs.ServiceType;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.ServiceTypes.Queries;

public class GetServiceTypeByIdQueryHandler : IRequestHandler<GetServiceTypeByIdQuery, ServiceTypeDto?>
{
    private readonly IRepository<ServiceType> _categoryRepository;
    private readonly IRepository<TargetPopulation> _publicRepository;

    public GetServiceTypeByIdQueryHandler(
        IRepository<ServiceType> categoryRepository,
        IRepository<TargetPopulation> publicRepository)
    {
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<ServiceTypeDto?> Handle(GetServiceTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category is null)
            return null;

        var dto = category.Adapt<ServiceTypeDto>();
        var facilityPublic = await _publicRepository.GetByIdAsync(category.TargetPopulationId, cancellationToken);
        dto.TargetPopulationName = facilityPublic?.Name ?? string.Empty;

        return dto;
    }
}
