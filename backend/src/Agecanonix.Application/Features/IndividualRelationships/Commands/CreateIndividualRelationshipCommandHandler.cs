using Agecanonix.Application.DTOs.IndividualRelationship;
using Agecanonix.Application.Interfaces;
using Agecanonix.Application.Services;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Commands;

public class CreateIndividualRelationshipCommandHandler : IRequestHandler<CreateIndividualRelationshipCommand, IndividualRelationshipDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly PriorityManagementService _priorityService;

    public CreateIndividualRelationshipCommandHandler(
        IUnitOfWork unitOfWork,
        PriorityManagementService priorityService)
    {
        _unitOfWork = unitOfWork;
        _priorityService = priorityService;
    }

    public async Task<IndividualRelationshipDto> Handle(CreateIndividualRelationshipCommand request, CancellationToken cancellationToken)
    {
        var relationship = request.Dto.Adapt<IndividualRelationship>();
        
        // Validate and assign priority (handles conflicts and ensures sequential priorities)
        relationship.Priority = await _priorityService.ValidateAndAssignPriorityForCreateAsync(
            request.Dto.SourceIndividualId,
            request.Dto.Priority,
            cancellationToken);
        
        relationship.CreatedAt = DateTime.UtcNow;
        relationship.CreatedBy = "system";

        await _unitOfWork.Relationships.AddAsync(relationship, cancellationToken);
        await _unitOfWork.CompleteAsync(cancellationToken);
        
        return relationship.Adapt<IndividualRelationshipDto>();
    }
}
