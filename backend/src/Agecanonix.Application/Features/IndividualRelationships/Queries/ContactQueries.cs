using Agecanonix.Application.DTOs.IndividualRelationship;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Queries;

public record GetAllIndividualRelationshipsQuery : IRequest<IEnumerable<IndividualRelationshipDto>>;
public record GetIndividualRelationshipByIdQuery(Guid Id) : IRequest<IndividualRelationshipDto?>;
public record GetIndividualRelationshipsByIndividualIdQuery(Guid IndividualId) : IRequest<IEnumerable<IndividualRelationshipDto>>;
