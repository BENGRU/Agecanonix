using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;

namespace Agecanonix.Application.Services;

/// <summary>
/// Service to manage priority assignment and reorganization for IndividualRelationships
/// Ensures priorities are sequential (1, 2, 3...) without gaps for each SourceIndividualId
/// </summary>
public class PriorityManagementService
{
    private readonly IUnitOfWork _unitOfWork;

    public PriorityManagementService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Validates and adjusts priority for a new relationship
    /// If priority is 0 or null, assigns the next available priority
    /// If priority conflicts, shifts existing relationships down
    /// </summary>
    public async Task<int> ValidateAndAssignPriorityForCreateAsync(
        Guid sourceIndividualId, 
        int requestedPriority, 
        CancellationToken cancellationToken = default)
    {
        var existingRelationships = (await _unitOfWork.Relationships.FindAsync(
            r => r.SourceIndividualId == sourceIndividualId,
            cancellationToken))
            .OrderBy(r => r.Priority)
            .ToList();

        // If no relationships exist, start at 1
        if (!existingRelationships.Any())
        {
            return 1;
        }

        // If priority is 0 or negative, assign next available
        if (requestedPriority <= 0)
        {
            return existingRelationships.Max(r => r.Priority) + 1;
        }

        // If requested priority is higher than max + 1, assign max + 1
        int maxPriority = existingRelationships.Max(r => r.Priority);
        if (requestedPriority > maxPriority + 1)
        {
            return maxPriority + 1;
        }

        // If priority conflicts with existing, shift down
        if (existingRelationships.Any(r => r.Priority == requestedPriority))
        {
            await ShiftPrioritiesDownAsync(sourceIndividualId, requestedPriority, cancellationToken);
        }

        return requestedPriority;
    }

    /// <summary>
    /// Validates and adjusts priority for an updated relationship
    /// </summary>
    public async Task ValidateAndAssignPriorityForUpdateAsync(
        Guid relationshipId,
        Guid sourceIndividualId,
        int newPriority,
        int oldPriority,
        CancellationToken cancellationToken = default)
    {
        if (newPriority == oldPriority)
        {
            return; // No change needed
        }

        var existingRelationships = (await _unitOfWork.Relationships.FindAsync(
            r => r.SourceIndividualId == sourceIndividualId && r.Id != relationshipId,
            cancellationToken))
            .OrderBy(r => r.Priority)
            .ToList();

        // Ensure priority starts at 1
        if (newPriority < 1)
        {
            throw new ArgumentException("Priority must be at least 1", nameof(newPriority));
        }

        // If new priority is higher than max + 1, throw error
        if (existingRelationships.Any())
        {
            int maxPriority = existingRelationships.Max(r => r.Priority);
            if (newPriority > maxPriority + 1)
            {
                throw new ArgumentException($"Priority cannot exceed {maxPriority + 1}", nameof(newPriority));
            }
        }
        else
        {
            // If no other relationships, must be 1
            if (newPriority != 1)
            {
                throw new ArgumentException("Priority must be 1 when it's the only relationship", nameof(newPriority));
            }
        }

        // Reorganize priorities
        await ReorganizePrioritiesForUpdateAsync(sourceIndividualId, relationshipId, oldPriority, newPriority, cancellationToken);
    }

    /// <summary>
    /// Reorganizes priorities after a relationship deletion to fill gaps
    /// </summary>
    public async Task ReorganizePrioritiesAfterDeleteAsync(
        Guid sourceIndividualId,
        int deletedPriority,
        CancellationToken cancellationToken = default)
    {
        var relationships = (await _unitOfWork.Relationships.FindAsync(
            r => r.SourceIndividualId == sourceIndividualId && r.Priority > deletedPriority,
            cancellationToken))
            .OrderBy(r => r.Priority)
            .ToList();

        foreach (var relationship in relationships)
        {
            relationship.Priority -= 1;
            relationship.UpdatedAt = DateTime.UtcNow;
            relationship.UpdatedBy = "system";
            await _unitOfWork.Relationships.UpdateAsync(relationship, cancellationToken);
        }
    }

    /// <summary>
    /// Shifts priorities down starting from the specified priority
    /// </summary>
    private async Task ShiftPrioritiesDownAsync(
        Guid sourceIndividualId,
        int startingPriority,
        CancellationToken cancellationToken)
    {
        var relationships = (await _unitOfWork.Relationships.FindAsync(
            r => r.SourceIndividualId == sourceIndividualId && r.Priority >= startingPriority,
            cancellationToken))
            .OrderByDescending(r => r.Priority)
            .ToList();

        foreach (var relationship in relationships)
        {
            relationship.Priority += 1;
            relationship.UpdatedAt = DateTime.UtcNow;
            relationship.UpdatedBy = "system";
            await _unitOfWork.Relationships.UpdateAsync(relationship, cancellationToken);
        }
    }

    /// <summary>
    /// Reorganizes priorities when updating a relationship
    /// </summary>
    private async Task ReorganizePrioritiesForUpdateAsync(
        Guid sourceIndividualId,
        Guid relationshipId,
        int oldPriority,
        int newPriority,
        CancellationToken cancellationToken)
    {
        if (newPriority > oldPriority)
        {
            // Moving down: shift up relationships between old and new position
            var relationships = (await _unitOfWork.Relationships.FindAsync(
                r => r.SourceIndividualId == sourceIndividualId 
                    && r.Id != relationshipId
                    && r.Priority > oldPriority 
                    && r.Priority <= newPriority,
                cancellationToken))
                .OrderBy(r => r.Priority)
                .ToList();

            foreach (var relationship in relationships)
            {
                relationship.Priority -= 1;
                relationship.UpdatedAt = DateTime.UtcNow;
                relationship.UpdatedBy = "system";
                await _unitOfWork.Relationships.UpdateAsync(relationship, cancellationToken);
            }
        }
        else
        {
            // Moving up: shift down relationships between new and old position
            var relationships = (await _unitOfWork.Relationships.FindAsync(
                r => r.SourceIndividualId == sourceIndividualId 
                    && r.Id != relationshipId
                    && r.Priority >= newPriority 
                    && r.Priority < oldPriority,
                cancellationToken))
                .OrderByDescending(r => r.Priority)
                .ToList();

            foreach (var relationship in relationships)
            {
                relationship.Priority += 1;
                relationship.UpdatedAt = DateTime.UtcNow;
                relationship.UpdatedBy = "system";
                await _unitOfWork.Relationships.UpdateAsync(relationship, cancellationToken);
            }
        }
    }
}
