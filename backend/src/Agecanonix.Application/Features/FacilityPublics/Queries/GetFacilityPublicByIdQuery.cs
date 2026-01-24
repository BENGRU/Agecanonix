using Agecanonix.Application.DTOs.FacilityPublic;
using MediatR;

namespace Agecanonix.Application.Features.FacilityPublics.Queries;

public record GetFacilityPublicByIdQuery(Guid Id) : IRequest<FacilityPublicDto?>;
