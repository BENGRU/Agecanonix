using Agecanonix.Application.DTOs.TargetPopulation;
using Agecanonix.Application.Exceptions;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Agecanonix.Application.Features.TargetPopulations.Commands;

public class UpdateTargetPopulationCommandHandler : IRequestHandler<UpdateTargetPopulationCommand, TargetPopulationDto>
{
    private readonly IRepository<TargetPopulation> _publicRepository;

    public UpdateTargetPopulationCommandHandler(IRepository<TargetPopulation> publicRepository)
    {
        _publicRepository = publicRepository;
    }

    public async Task<TargetPopulationDto> Handle(UpdateTargetPopulationCommand request, CancellationToken cancellationToken)
    {
        var entity = await _publicRepository.GetByIdAsync(request.Id, cancellationToken)
                     ?? throw new KeyNotFoundException($"Facility public with ID {request.Id} not found");

        // Set the row version from the DTO for optimistic concurrency control
        entity.RowVersion = request.Dto.RowVersion;

        try
        {
            request.Dto.Adapt(entity);
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = "system";

            await _publicRepository.UpdateAsync(entity, cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyException(
                $"Unable to update TargetPopulation with ID {request.Id}. " +
                $"The record was modified by another user. Please refresh and try again.",
                ex
            );
        }

        return entity.Adapt<TargetPopulationDto>();
    }
}
