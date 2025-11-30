using Agecanonix.Application.DTOs.IndividualRelationship;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Commands;

public record CreateIndividualRelationshipCommand(CreateIndividualRelationshipDto Dto) : IRequest<IndividualRelationshipDto>;
public record UpdateIndividualRelationshipCommand(Guid Id, UpdateIndividualRelationshipDto Dto) : IRequest<IndividualRelationshipDto>;
public record DeleteIndividualRelationshipCommand(Guid Id) : IRequest<bool>;
