using Agecanonix.Application.DTOs.TargetPopulation;
using MediatR;

namespace Agecanonix.Application.Features.TargetPopulations.Queries;

public record GetTargetPopulationByIdQuery(Guid Id) : IRequest<TargetPopulationDto?>;
