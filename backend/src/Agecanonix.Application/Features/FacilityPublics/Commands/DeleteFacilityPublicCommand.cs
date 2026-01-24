using MediatR;

namespace Agecanonix.Application.Features.FacilityPublics.Commands;

public record DeleteFacilityPublicCommand(Guid Id) : IRequest<bool>;
