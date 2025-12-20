using Agecanonix.Application.DTOs.Facility;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public class CreateFacilityCommandHandler : IRequestHandler<CreateFacilityCommand, FacilityDto>
{
    private readonly IRepository<Facility> _repository;

    public CreateFacilityCommandHandler(IRepository<Facility> repository)
    {
        _repository = repository;
    }

    public async Task<FacilityDto> Handle(CreateFacilityCommand request, CancellationToken cancellationToken)
    {
        var facility = request.Dto.Adapt<Facility>();
        facility.Id = Guid.NewGuid();
        facility.CreatedAt = DateTime.UtcNow;
        facility.CreatedBy = "system"; // TODO: Get from authenticated user

        var created = await _repository.AddAsync(facility, cancellationToken);
        return created.Adapt<FacilityDto>();
    }
}
