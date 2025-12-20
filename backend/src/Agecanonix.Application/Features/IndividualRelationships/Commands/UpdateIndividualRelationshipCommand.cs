using Agecanonix.Application.DTOs.IndividualRelationship;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Commands;

public record UpdateIndividualRelationshipCommand(Guid Id, UpdateIndividualRelationshipDto Dto) : IRequest<IndividualRelationshipDto>;
