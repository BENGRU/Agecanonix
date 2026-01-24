using Agecanonix.Application.DTOs.ServiceType;
using MediatR;

namespace Agecanonix.Application.Features.ServiceTypes.Commands;

public record UpdateServiceTypeCommand(Guid Id, UpdateServiceTypeDto Dto) : IRequest<ServiceTypeDto>;
