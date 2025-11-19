using Agecanonix.Application.DTOs.Facility;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Queries;

public record GetFacilityByIdQuery(Guid Id) : IRequest<FacilityDto?>;
