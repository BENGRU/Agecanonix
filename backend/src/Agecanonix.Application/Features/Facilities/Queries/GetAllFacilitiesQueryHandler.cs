using System.Linq;
using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Queries;

public class GetAllFacilitiesQueryHandler : IRequestHandler<GetAllFacilitiesQuery, IEnumerable<FacilityDto>>
{
    private readonly IRepository<Facility> _repository;
    private readonly IRepository<ServiceType> _categoryRepository;
    private readonly IRepository<TargetPopulation> _publicRepository;

    public GetAllFacilitiesQueryHandler(
        IRepository<Facility> repository,
        IRepository<ServiceType> categoryRepository,
        IRepository<TargetPopulation> publicRepository)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<IEnumerable<FacilityDto>> Handle(GetAllFacilitiesQuery request, CancellationToken cancellationToken)
    {
        var facilities = (await _repository.GetAllAsync(cancellationToken)).ToList();
        if (!facilities.Any())
            return Enumerable.Empty<FacilityDto>();

        var categoryIds = facilities
            .Select(f => f.ServiceTypeId)
            .Distinct()
            .ToList();

        var categories = (await _categoryRepository.FindAsync(c => categoryIds.Contains(c.Id), cancellationToken)).ToList();
        var categoryLookup = categories.ToDictionary(c => c.Id, c => new { c.Name, c.TargetPopulationId });

        var publicIds = categories.Select(c => c.TargetPopulationId).Distinct().ToList();
        var publics = await _publicRepository.FindAsync(p => publicIds.Contains(p.Id), cancellationToken);
        var publicLookup = publics.ToDictionary(p => p.Id, p => p.Name);

        return facilities.Select(facility =>
        {
            var dto = facility.Adapt<FacilityDto>();
            if (categoryLookup.TryGetValue(facility.ServiceTypeId, out var catInfo))
            {
                dto.ServiceTypeName = catInfo.Name;
                dto.TargetPopulationId = catInfo.TargetPopulationId;
                if (publicLookup.TryGetValue(catInfo.TargetPopulationId, out var publicName))
                {
                    dto.TargetPopulationName = publicName;
                }
            }

            return dto;
        });
    }
}
