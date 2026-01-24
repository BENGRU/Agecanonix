using MediatR;

namespace Agecanonix.Application.Features.TargetPopulations.Commands;

public record DeleteTargetPopulationCommand(Guid Id) : IRequest<bool>;
