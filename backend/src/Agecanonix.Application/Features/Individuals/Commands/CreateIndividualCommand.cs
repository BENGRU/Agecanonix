using Agecanonix.Application.DTOs.Individual;
using MediatR;

namespace Agecanonix.Application.Features.Individuals.Commands;

public record CreateIndividualCommand(CreateIndividualDto Dto) : IRequest<IndividualDto>;
