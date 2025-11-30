using Agecanonix.Application.DTOs.Individual;
using MediatR;

namespace Agecanonix.Application.Features.Individuals.Commands;

public record CreateIndividualCommand(CreateIndividualDto Dto) : IRequest<IndividualDto>;

public record UpdateIndividualCommand(Guid Id, UpdateIndividualDto Dto) : IRequest<IndividualDto>;

public record DeleteIndividualCommand(Guid Id) : IRequest<bool>;
