using Agecanonix.Application.DTOs.Individual;
using MediatR;

namespace Agecanonix.Application.Features.Individuals.Commands;

public record UpdateIndividualCommand(Guid Id, UpdateIndividualDto Dto) : IRequest<IndividualDto>;
