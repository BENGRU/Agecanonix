using Agecanonix.Application.DTOs.Facility;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Queries;

public record GetAllFacilitiesQuery : IRequest<IEnumerable<FacilityDto>>;
