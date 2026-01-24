using Agecanonix.Application.DTOs.TargetPopulation;
using MediatR;

namespace Agecanonix.Application.Features.TargetPopulations.Commands;

public record CreateTargetPopulationCommand(CreateTargetPopulationDto Dto) : IRequest<TargetPopulationDto>;
