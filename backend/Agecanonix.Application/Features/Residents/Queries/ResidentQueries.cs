using Agecanonix.Application.DTOs.Resident;
using MediatR;

namespace Agecanonix.Application.Features.Residents.Queries;

public record GetAllResidentsQuery : IRequest<IEnumerable<ResidentDto>>;

public record GetResidentByIdQuery(Guid Id) : IRequest<ResidentDto?>;
