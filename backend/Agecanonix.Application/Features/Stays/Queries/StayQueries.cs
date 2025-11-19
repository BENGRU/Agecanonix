using Agecanonix.Application.DTOs.Stay;
using MediatR;

namespace Agecanonix.Application.Features.Stays.Queries;

public record GetAllStaysQuery : IRequest<IEnumerable<StayDto>>;
public record GetStayByIdQuery(Guid Id) : IRequest<StayDto?>;
public record GetStaysByResidentIdQuery(Guid ResidentId) : IRequest<IEnumerable<StayDto>>;
public record GetStaysByFacilityIdQuery(Guid FacilityId) : IRequest<IEnumerable<StayDto>>;
