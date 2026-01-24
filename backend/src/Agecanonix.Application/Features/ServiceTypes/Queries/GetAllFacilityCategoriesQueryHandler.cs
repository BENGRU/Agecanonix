using System.Linq;
using Agecanonix.Application.DTOs.ServiceType;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.ServiceTypes.Queries;

public class GetAllServiceTypesQueryHandler : IRequestHandler<GetAllServiceTypesQuery, IEnumerable<ServiceTypeDto>>
{
    private readonly IRepository<ServiceType> _categoryRepository;
    private readonly IRepository<TargetPopulation> _publicRepository;

    public GetAllServiceTypesQueryHandler(
        IRepository<ServiceType> categoryRepository,
        IRepository<TargetPopulation> publicRepository)
    {
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<IEnumerable<ServiceTypeDto>> Handle(GetAllServiceTypesQuery request, CancellationToken cancellationToken)
    {
        var categories = (await _categoryRepository.GetAllAsync(cancellationToken)).ToList();
        if (!categories.Any())
            return Enumerable.Empty<ServiceTypeDto>();

        var publicIds = categories.Select(c => c.TargetPopulationId).Distinct().ToList();
        var publics = await _publicRepository.FindAsync(p => publicIds.Contains(p.Id), cancellationToken);
        var publicLookup = publics.ToDictionary(p => p.Id, p => p.Name);

        return categories.Select(category =>
        {
            var dto = category.Adapt<ServiceTypeDto>();
            if (publicLookup.TryGetValue(category.TargetPopulationId, out var publicName))
            {
                dto.TargetPopulationName = publicName;
            }
            return dto;
        });
    }
}
