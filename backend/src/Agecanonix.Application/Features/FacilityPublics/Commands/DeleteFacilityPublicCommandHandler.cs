using System.Linq;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using MediatR;

namespace Agecanonix.Application.Features.FacilityPublics.Commands;

public class DeleteFacilityPublicCommandHandler : IRequestHandler<DeleteFacilityPublicCommand, bool>
{
    private readonly IRepository<FacilityPublic> _publicRepository;
    private readonly IRepository<FacilityCategory> _categoryRepository;

    public DeleteFacilityPublicCommandHandler(
        IRepository<FacilityPublic> publicRepository,
        IRepository<FacilityCategory> categoryRepository)
    {
        _publicRepository = publicRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(DeleteFacilityPublicCommand request, CancellationToken cancellationToken)
    {
        var entity = await _publicRepository.GetByIdAsync(request.Id, cancellationToken);
        if (entity is null)
            return false;

        var hasCategories = (await _categoryRepository.FindAsync(c => c.FacilityPublicId == request.Id, cancellationToken)).Any();
        if (hasCategories)
            throw new InvalidOperationException("Cannot delete a public that has categories.");

        await _publicRepository.DeleteAsync(entity, cancellationToken);
        return true;
    }
}
