using System.Linq;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using MediatR;

namespace Agecanonix.Application.Features.FacilityCategories.Commands;

public class DeleteFacilityCategoryCommandHandler : IRequestHandler<DeleteFacilityCategoryCommand, bool>
{
    private readonly IRepository<FacilityCategory> _categoryRepository;
    private readonly IRepository<Facility> _facilityRepository;

    public DeleteFacilityCategoryCommandHandler(
        IRepository<FacilityCategory> categoryRepository,
        IRepository<Facility> facilityRepository)
    {
        _categoryRepository = categoryRepository;
        _facilityRepository = facilityRepository;
    }

    public async Task<bool> Handle(DeleteFacilityCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        if (category == null)
            return false;

        var hasFacilities = (await _facilityRepository.FindAsync(f => f.FacilityCategoryId == request.Id, cancellationToken)).Any();
        if (hasFacilities)
            throw new InvalidOperationException("Cannot delete a category that is assigned to facilities.");

        await _categoryRepository.DeleteAsync(category, cancellationToken);
        return true;
    }
}
