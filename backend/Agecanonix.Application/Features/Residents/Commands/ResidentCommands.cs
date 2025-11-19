using Agecanonix.Application.DTOs.Resident;
using MediatR;

namespace Agecanonix.Application.Features.Residents.Commands;

public record CreateResidentCommand(CreateResidentDto Dto) : IRequest<ResidentDto>;

public record UpdateResidentCommand(Guid Id, UpdateResidentDto Dto) : IRequest<ResidentDto>;

public record DeleteResidentCommand(Guid Id) : IRequest<bool>;
