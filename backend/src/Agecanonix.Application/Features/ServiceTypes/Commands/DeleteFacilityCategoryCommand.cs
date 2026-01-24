using MediatR;

namespace Agecanonix.Application.Features.ServiceTypes.Commands;

public record DeleteServiceTypeCommand(Guid Id) : IRequest<bool>;
