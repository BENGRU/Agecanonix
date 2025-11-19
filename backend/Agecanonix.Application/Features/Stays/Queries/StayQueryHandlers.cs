using Agecanonix.Application.DTOs.Stay;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Stays.Queries;

public class GetAllStaysQueryHandler : IRequestHandler<GetAllStaysQuery, IEnumerable<StayDto>>
{
    private readonly IRepository<Stay> _repository;
    private readonly IMapper _mapper;

    public GetAllStaysQueryHandler(IRepository<Stay> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StayDto>> Handle(GetAllStaysQuery request, CancellationToken cancellationToken)
    {
        var stays = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<StayDto>>(stays);
    }
}

public class GetStayByIdQueryHandler : IRequestHandler<GetStayByIdQuery, StayDto?>
{
    private readonly IRepository<Stay> _repository;
    private readonly IMapper _mapper;

    public GetStayByIdQueryHandler(IRepository<Stay> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StayDto?> Handle(GetStayByIdQuery request, CancellationToken cancellationToken)
    {
        var stay = await _repository.GetByIdAsync(request.Id);
        return stay == null ? null : _mapper.Map<StayDto>(stay);
    }
}

public class GetStaysByResidentIdQueryHandler : IRequestHandler<GetStaysByResidentIdQuery, IEnumerable<StayDto>>
{
    private readonly IRepository<Stay> _repository;
    private readonly IMapper _mapper;

    public GetStaysByResidentIdQueryHandler(IRepository<Stay> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StayDto>> Handle(GetStaysByResidentIdQuery request, CancellationToken cancellationToken)
    {
        var stays = await _repository.FindAsync(s => s.ResidentId == request.ResidentId);
        return _mapper.Map<IEnumerable<StayDto>>(stays.OrderByDescending(s => s.StartDate));
    }
}

public class GetStaysByFacilityIdQueryHandler : IRequestHandler<GetStaysByFacilityIdQuery, IEnumerable<StayDto>>
{
    private readonly IRepository<Stay> _repository;
    private readonly IMapper _mapper;

    public GetStaysByFacilityIdQueryHandler(IRepository<Stay> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<StayDto>> Handle(GetStaysByFacilityIdQuery request, CancellationToken cancellationToken)
    {
        var stays = await _repository.FindAsync(s => s.FacilityId == request.FacilityId);
        return _mapper.Map<IEnumerable<StayDto>>(stays.OrderByDescending(s => s.StartDate));
    }
}
