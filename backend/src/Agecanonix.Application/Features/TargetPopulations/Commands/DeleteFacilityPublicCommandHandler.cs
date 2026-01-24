using System.Linq;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using MediatR;

namespace Agecanonix.Application.Features.TargetPopulations.Commands;

public class DeleteTargetPopulationCommandHandler : IRequestHandler<DeleteTargetPopulationCommand, bool>
{
    private readonly IRepository<TargetPopulation> _publicRepository;
    private readonly IRepository<ServiceType> _categoryRepository;

    public DeleteTargetPopulationCommandHandler(
        IRepository<TargetPopulation> publicRepository,
        IRepository<ServiceType> categoryRepository)
    {
        _publicRepository = publicRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(DeleteTargetPopulationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _publicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return false;

        var hasCategories = (await _categoryRepository.FindAsync(c => c.TargetPopulationId == request.Id, cancellationToken)).Any();
        if (hasCategories)
            throw new InvalidOperationException("Cannot delete a public that has categories.");

        await _publicRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }
}
