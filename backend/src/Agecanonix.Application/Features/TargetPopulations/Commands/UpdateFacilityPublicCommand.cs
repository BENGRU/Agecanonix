using Agecanonix.Application.DTOs.TargetPopulation;
using MediatR;

namespace Agecanonix.Application.Features.TargetPopulations.Commands;

public record UpdateTargetPopulationCommand(Guid Id, UpdateTargetPopulationDto Dto) : IRequest<TargetPopulationDto>;
