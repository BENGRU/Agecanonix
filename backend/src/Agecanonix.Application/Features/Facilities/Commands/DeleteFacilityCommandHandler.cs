using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using MediatR;

namespace Agecanonix.Application.Features.Facilities.Commands;

public class DeleteFacilityCommandHandler : IRequestHandler<DeleteFacilityCommand, bool>
{
    private readonly IRepository<Facility> _repository;

    public DeleteFacilityCommandHandler(IRepository<Facility> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteFacilityCommand request, CancellationToken cancellationToken)
    {
        var facility = await _repository.GetByIdAsync(request.Id);
        if (facility == null)
            return false;

        await _repository.DeleteAsync(facility);
        return true;
    }
}
