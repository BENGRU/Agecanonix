using Agecanonix.Application.Interfaces;
using Agecanonix.Application.Services;
using Agecanonix.Domain.Entities;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Commands;

public class DeleteIndividualRelationshipCommandHandler : IRequestHandler<DeleteIndividualRelationshipCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PriorityManagementService _priorityService;

    public DeleteIndividualRelationshipCommandHandler(
        IUnitOfWork unitOfWork,
        PriorityManagementService priorityService)
    {
        _unitOfWork = unitOfWork;
        _priorityService = priorityService;
    }

    public async Task<bool> Handle(DeleteIndividualRelationshipCommand request, CancellationToken cancellationToken)
    {
        var relationship = await _unitOfWork.Relationships.GetByIdAsync(request.Id, cancellationToken);
        if (relationship == null)
            return false;

        Guid sourceIndividualId = relationship.SourceIndividualId;
        int deletedPriority = relationship.Priority;
        
        await _unitOfWork.Relationships.DeleteAsync(relationship, cancellationToken);
        
        // Reorganize priorities to fill the gap
        await _priorityService.ReorganizePrioritiesAfterDeleteAsync(
            sourceIndividualId,
            deletedPriority,
            cancellationToken);
        
        await _unitOfWork.CompleteAsync(cancellationToken);
        
        return true;
    }
}
