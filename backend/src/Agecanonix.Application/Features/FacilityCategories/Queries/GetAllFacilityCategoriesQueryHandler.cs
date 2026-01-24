using System.Linq;
using Agecanonix.Application.DTOs.FacilityCategory;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.FacilityCategories.Queries;

public class GetAllFacilityCategoriesQueryHandler : IRequestHandler<GetAllFacilityCategoriesQuery, IEnumerable<FacilityCategoryDto>>
{
    private readonly IRepository<FacilityCategory> _categoryRepository;
    private readonly IRepository<FacilityPublic> _publicRepository;

    public GetAllFacilityCategoriesQueryHandler(
        IRepository<FacilityCategory> categoryRepository,
        IRepository<FacilityPublic> publicRepository)
    {
        _categoryRepository = categoryRepository;
        _publicRepository = publicRepository;
    }

    public async Task<IEnumerable<FacilityCategoryDto>> Handle(GetAllFacilityCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = (await _categoryRepository.GetAllAsync(cancellationToken)).ToList();
        if (!categories.Any())
            return Enumerable.Empty<FacilityCategoryDto>();

        var publicIds = categories.Select(c => c.FacilityPublicId).Distinct().ToList();
        var publics = await _publicRepository.FindAsync(p => publicIds.Contains(p.Id), cancellationToken);
        var publicLookup = publics.ToDictionary(p => p.Id, p => p.Name);

        return categories.Select(category =>
        {
            var dto = category.Adapt<FacilityCategoryDto>();
            if (publicLookup.TryGetValue(category.FacilityPublicId, out var publicName))
            {
                dto.FacilityPublicName = publicName;
            }
            return dto;
        });
    }
}
