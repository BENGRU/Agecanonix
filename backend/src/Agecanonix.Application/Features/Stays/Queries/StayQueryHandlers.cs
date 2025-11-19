using Agecanonix.Application.DTOs.Stay;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Stays.Queries;

public class GetAllStaysQueryHandler : IRequestHandler<GetAllStaysQuery, IEnumerable<StayDto>>
{
    private readonly IRepository<Stay> _repository;

    public GetAllStaysQueryHandler(IRepository<Stay> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<StayDto>> Handle(GetAllStaysQuery request, CancellationToken cancellationToken)
    {
        var stays = await _repository.GetAllAsync();
        return stays.Adapt<IEnumerable<StayDto>>();
    }
}

public class GetStayByIdQueryHandler : IRequestHandler<GetStayByIdQuery, StayDto?>
{
    private readonly IRepository<Stay> _repository;

    public GetStayByIdQueryHandler(IRepository<Stay> repository)
    {
        _repository = repository;
    }

    public async Task<StayDto?> Handle(GetStayByIdQuery request, CancellationToken cancellationToken)
    {
        var stay = await _repository.GetByIdAsync(request.Id);
        return stay?.Adapt<StayDto>();
    }
}

public class GetStaysByResidentIdQueryHandler : IRequestHandler<GetStaysByResidentIdQuery, IEnumerable<StayDto>>
{
    private readonly IRepository<Stay> _repository;

    public GetStaysByResidentIdQueryHandler(IRepository<Stay> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<StayDto>> Handle(GetStaysByResidentIdQuery request, CancellationToken cancellationToken)
    {
        var stays = await _repository.FindAsync(s => s.ResidentId == request.ResidentId);
        return stays.OrderByDescending(s => s.StartDate).Adapt<IEnumerable<StayDto>>();
    }
}

public class GetStaysByFacilityIdQueryHandler : IRequestHandler<GetStaysByFacilityIdQuery, IEnumerable<StayDto>>
{
    private readonly IRepository<Stay> _repository;

    public GetStaysByFacilityIdQueryHandler(IRepository<Stay> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<StayDto>> Handle(GetStaysByFacilityIdQuery request, CancellationToken cancellationToken)
    {
        var stays = await _repository.FindAsync(s => s.FacilityId == request.FacilityId);
        return stays.OrderByDescending(s => s.StartDate).Adapt<IEnumerable<StayDto>>();
    }
}
