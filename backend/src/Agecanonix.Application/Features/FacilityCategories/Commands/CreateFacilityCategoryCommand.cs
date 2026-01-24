using Agecanonix.Application.DTOs.FacilityCategory;
using MediatR;

namespace Agecanonix.Application.Features.FacilityCategories.Commands;

public record CreateFacilityCategoryCommand(CreateFacilityCategoryDto Dto) : IRequest<FacilityCategoryDto>;
