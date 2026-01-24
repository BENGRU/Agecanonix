using Agecanonix.Application.DTOs.ServiceType;
using MediatR;

namespace Agecanonix.Application.Features.ServiceTypes.Commands;

public record CreateServiceTypeCommand(CreateServiceTypeDto Dto) : IRequest<ServiceTypeDto>;
