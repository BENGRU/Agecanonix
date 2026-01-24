using Agecanonix.Application.DTOs.FacilityCategory;
using MediatR;

namespace Agecanonix.Application.Features.FacilityCategories.Commands;

public record UpdateFacilityCategoryCommand(Guid Id, UpdateFacilityCategoryDto Dto) : IRequest<FacilityCategoryDto>;
