using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public record UpdateFacilityCommand(Guid Id, DTOs.Facility.UpdateFacilityDto Dto) : IRequest<DTOs.Facility.FacilityDto>;
