using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public class CreateFacilityCommandHandler : IRequestHandler<CreateFacilityCommand, FacilityDto>
{
    private readonly IRepository<Facility> _repository;
    private readonly IMapper _mapper;

    public CreateFacilityCommandHandler(IRepository<Facility> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FacilityDto> Handle(CreateFacilityCommand request, CancellationToken cancellationToken)
    {
        var facility = _mapper.Map<Facility>(request.Dto);
        facility.Id = Guid.NewGuid();
        facility.CreatedAt = DateTime.UtcNow;
        facility.CreatedBy = "system"; // TODO: Get from authenticated user

        var created = await _repository.AddAsync(facility);
        return _mapper.Map<FacilityDto>(created);
    }
}
