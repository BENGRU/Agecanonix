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
    private readonly IRepository<FacilityCategory> _categoryRepository;
    private readonly IRepository<FacilityPublic> _publicRepository;

    public GetAllFacilitiesQueryHandler(
        IRepository<Facility> repository,
        IRepository<FacilityCategory> categoryRepository,
        IRepository<FacilityPublic> publicRepository)
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
            .Select(f => f.FacilityCategoryId)
            .Distinct()
            .ToList();

        var categories = (await _categoryRepository.FindAsync(c => categoryIds.Contains(c.Id), cancellationToken)).ToList();
        var categoryLookup = categories.ToDictionary(c => c.Id, c => new { c.Name, c.FacilityPublicId });

        var publicIds = categories.Select(c => c.FacilityPublicId).Distinct().ToList();
        var publics = await _publicRepository.FindAsync(p => publicIds.Contains(p.Id), cancellationToken);
        var publicLookup = publics.ToDictionary(p => p.Id, p => p.Name);

        return facilities.Select(facility =>
        {
            var dto = facility.Adapt<FacilityDto>();
            if (categoryLookup.TryGetValue(facility.FacilityCategoryId, out var catInfo))
            {
                dto.FacilityCategoryName = catInfo.Name;
                dto.FacilityPublicId = catInfo.FacilityPublicId;
                if (publicLookup.TryGetValue(catInfo.FacilityPublicId, out var publicName))
                {
                    dto.FacilityPublicName = publicName;
                }
            }

            return dto;
        });
    }
}
