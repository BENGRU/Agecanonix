using Agecanonix.Application.DTOs.IndividualRelationship;
using Agecanonix.Application.Interfaces;
using Agecanonix.Application.Services;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Commands;

public class UpdateIndividualRelationshipCommandHandler : IRequestHandler<UpdateIndividualRelationshipCommand, IndividualRelationshipDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PriorityManagementService _priorityService;

    public UpdateIndividualRelationshipCommandHandler(
        IUnitOfWork unitOfWork,
        PriorityManagementService priorityService)
    {
        _unitOfWork = unitOfWork;
        _priorityService = priorityService;
    }

    public async Task<IndividualRelationshipDto> Handle(UpdateIndividualRelationshipCommand request, CancellationToken cancellationToken)
    {
        var relationship = await _unitOfWork.Relationships.GetByIdAsync(request.Id, cancellationToken);
        if (relationship == null)
            throw new KeyNotFoundException($"IndividualRelationship with ID {request.Id} not found");

        int oldPriority = relationship.Priority;
        Guid sourceIndividualId = relationship.SourceIndividualId;
        
        request.Dto.Adapt(relationship);
        
        // Validate and reorganize priorities if priority changed
        if (request.Dto.Priority != oldPriority)
        {
            await _priorityService.ValidateAndAssignPriorityForUpdateAsync(
                request.Id,
                sourceIndividualId,
                request.Dto.Priority,
                oldPriority,
                cancellationToken);
        }
        
        relationship.UpdatedAt = DateTime.UtcNow;
        relationship.UpdatedBy = "system";

        await _unitOfWork.Relationships.UpdateAsync(relationship, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        
        return relationship.Adapt<IndividualRelationshipDto>();
    }
}
