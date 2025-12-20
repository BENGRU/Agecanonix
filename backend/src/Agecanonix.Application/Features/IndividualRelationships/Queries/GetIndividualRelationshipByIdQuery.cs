using Agecanonix.Application.DTOs.IndividualRelationship;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Queries;

public record GetIndividualRelationshipByIdQuery(Guid Id) : IRequest<IndividualRelationshipDto?>;
