using MediatR;

namespace Agecanonix.Application.Features.Individuals.Commands;

public record DeleteIndividualCommand(Guid Id) : IRequest<bool>;
