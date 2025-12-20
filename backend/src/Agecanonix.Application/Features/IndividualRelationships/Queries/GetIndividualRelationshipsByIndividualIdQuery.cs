using Agecanonix.Application.DTOs.IndividualRelationship;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Queries;

public record GetIndividualRelationshipsByIndividualIdQuery(Guid IndividualId) : IRequest<IEnumerable<IndividualRelationshipDto>>;
