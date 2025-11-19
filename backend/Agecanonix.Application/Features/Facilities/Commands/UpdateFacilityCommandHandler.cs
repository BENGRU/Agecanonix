using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, FacilityDto>
{
    private readonly IRepository<Facility> _repository;
    private readonly IMapper _mapper;

    public UpdateFacilityCommandHandler(IRepository<Facility> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<FacilityDto> Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
    {
        var facility = await _repository.GetByIdAsync(request.Id);
        if (facility == null)
            throw new KeyNotFoundException($"Facility with ID {request.Id} not found");

        _mapper.Map(request.Dto, facility);
        facility.UpdatedAt = DateTime.UtcNow;
        facility.UpdatedBy = "system"; // TODO: Get from authenticated user

        await _repository.UpdateAsync(facility);
        return _mapper.Map<FacilityDto>(facility);
    }
}
