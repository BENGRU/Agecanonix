using Agecanonix.Application.DTOs.FacilityPublic;
using MediatR;

namespace Agecanonix.Application.Features.FacilityPublics.Commands;

public record UpdateFacilityPublicCommand(Guid Id, UpdateFacilityPublicDto Dto) : IRequest<FacilityPublicDto>;
