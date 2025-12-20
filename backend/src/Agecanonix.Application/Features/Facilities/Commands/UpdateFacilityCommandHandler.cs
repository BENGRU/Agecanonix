using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public class UpdateFacilityCommandHandler : IRequestHandler<UpdateFacilityCommand, FacilityDto>
{
    private readonly IRepository<Facility> _repository;

    public UpdateFacilityCommandHandler(IRepository<Facility> repository)
    {
        _repository = repository;
    }

    public async Task<FacilityDto> Handle(UpdateFacilityCommand request, CancellationToken cancellationToken)
    {
        var facility = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (facility == null)
            throw new KeyNotFoundException($"Facility with ID {request.Id} not found");

        request.Dto.Adapt(facility);
        facility.UpdatedAt = DateTime.UtcNow;
        facility.UpdatedBy = "system"; // TODO: Get from authenticated user

        await _repository.UpdateAsync(facility, cancellationToken);
        return facility.Adapt<FacilityDto>();
    }
}
