using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Commands;

public record DeleteIndividualRelationshipCommand(Guid Id) : IRequest<bool>;
