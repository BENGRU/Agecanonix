using Agecanonix.Application.DTOs.FacilityCategory;
using MediatR;

namespace Agecanonix.Application.Features.FacilityCategories.Queries;

public record GetAllFacilityCategoriesQuery : IRequest<IEnumerable<FacilityCategoryDto>>;
