using System.Linq;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using MediatR;

namespace Agecanonix.Application.Features.ServiceTypes.Commands;

public class DeleteServiceTypeCommandHandler : IRequestHandler<DeleteServiceTypeCommand, bool>
{
    private readonly IRepository<ServiceType> _categoryRepository;
    private readonly IRepository<Facility> _facilityRepository;

    public DeleteServiceTypeCommandHandler(
        IRepository<ServiceType> categoryRepository,
        IRepository<Facility> facilityRepository)
    {
        _categoryRepository = categoryRepository;
        _facilityRepository = facilityRepository;
    }

    public async Task<bool> Handle(DeleteServiceTypeCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
            return false;

        var hasFacilities = (await _facilityRepository.FindAsync(f => f.ServiceTypeId == request.Id, cancellationToken)).Any();
        if (hasFacilities)
            throw new InvalidOperationException("Cannot delete a category that is assigned to facilities.");

        await _categoryRepository.DeleteAsync(category, cancellationToken);
        return true;
    }
}
