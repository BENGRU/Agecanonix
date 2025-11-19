using Agecanonix.Application.DTOs.Stay;
using MediatR;

namespace Agecanonix.Application.Features.Stays.Commands;

public record CreateStayCommand(CreateStayDto Dto) : IRequest<StayDto>;
public record UpdateStayCommand(Guid Id, UpdateStayDto Dto) : IRequest<StayDto>;
public record DeleteStayCommand(Guid Id) : IRequest<bool>;
