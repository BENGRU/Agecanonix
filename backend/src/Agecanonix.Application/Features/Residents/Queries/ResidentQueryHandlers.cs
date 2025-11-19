using Agecanonix.Application.DTOs.Resident;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Residents.Queries;

public class GetAllResidentsQueryHandler : IRequestHandler<GetAllResidentsQuery, IEnumerable<ResidentDto>>
{
    private readonly IRepository<Resident> _repository;

    public GetAllResidentsQueryHandler(IRepository<Resident> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ResidentDto>> Handle(GetAllResidentsQuery request, CancellationToken cancellationToken)
    {
        var residents = await _repository.GetAllAsync();
        return residents.Adapt<IEnumerable<ResidentDto>>();
    }
}

public class GetResidentByIdQueryHandler : IRequestHandler<GetResidentByIdQuery, ResidentDto?>
{
    private readonly IRepository<Resident> _repository;

    public GetResidentByIdQueryHandler(IRepository<Resident> repository)
    {
        _repository = repository;
    }

    public async Task<ResidentDto?> Handle(GetResidentByIdQuery request, CancellationToken cancellationToken)
    {
        var resident = await _repository.GetByIdAsync(request.Id);
        return resident?.Adapt<ResidentDto>();
    }
}
