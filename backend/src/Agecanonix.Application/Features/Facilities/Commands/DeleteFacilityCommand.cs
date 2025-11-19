using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public record DeleteFacilityCommand(Guid Id) : IRequest<bool>;
