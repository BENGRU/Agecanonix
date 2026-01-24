using MediatR;

namespace Agecanonix.Application.Features.FacilityCategories.Commands;

public record DeleteFacilityCategoryCommand(Guid Id) : IRequest<bool>;
