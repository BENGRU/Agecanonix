using Agecanonix.Application.DTOs.Facility;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public record CreateFacilityCommand(CreateFacilityDto Dto) : IRequest<FacilityDto>;
