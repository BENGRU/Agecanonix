using Agecanonix.Application.DTOs.FacilityPublic;
using MediatR;

namespace Agecanonix.Application.Features.FacilityPublics.Commands;

public record CreateFacilityPublicCommand(CreateFacilityPublicDto Dto) : IRequest<FacilityPublicDto>;
