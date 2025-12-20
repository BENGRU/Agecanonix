using Agecanonix.Application.DTOs.Individual;
using MediatR;

namespace Agecanonix.Application.Features.Individuals.Queries;

public record GetIndividualByIdQuery(Guid Id) : IRequest<IndividualDto?>;
