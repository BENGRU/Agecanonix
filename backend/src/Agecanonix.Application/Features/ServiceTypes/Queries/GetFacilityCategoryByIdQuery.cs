using Agecanonix.Application.DTOs.ServiceType;
using MediatR;

namespace Agecanonix.Application.Features.ServiceTypes.Queries;

public record GetServiceTypeByIdQuery(Guid Id) : IRequest<ServiceTypeDto?>;
