using Agecanonix.Application.DTOs.Resident;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Residents.Queries;

public class GetAllResidentsQueryHandler : IRequestHandler<GetAllResidentsQuery, IEnumerable<ResidentDto>>
{
    private readonly IRepository<Resident> _repository;
    private readonly IMapper _mapper;

    public GetAllResidentsQueryHandler(IRepository<Resident> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ResidentDto>> Handle(GetAllResidentsQuery request, CancellationToken cancellationToken)
    {
        var residents = await _repository.GetAllAsync();
        return _mapper.Map<IEnumerable<ResidentDto>>(residents);
    }
}

public class GetResidentByIdQueryHandler : IRequestHandler<GetResidentByIdQuery, ResidentDto?>
{
    private readonly IRepository<Resident> _repository;
    private readonly IMapper _mapper;

    public GetResidentByIdQueryHandler(IRepository<Resident> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResidentDto?> Handle(GetResidentByIdQuery request, CancellationToken cancellationToken)
    {
        var resident = await _repository.GetByIdAsync(request.Id);
        return resident == null ? null : _mapper.Map<ResidentDto>(resident);
    }
}
