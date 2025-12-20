using Agecanonix.Application.DTOs.Individual;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Individuals.Commands;

public class UpdateIndividualCommandHandler : IRequestHandler<UpdateIndividualCommand, IndividualDto>
{
    private readonly IRepository<Individual> _repository;

    public UpdateIndividualCommandHandler(IRepository<Individual> repository)
    {
        _repository = repository;
    }

    public async Task<IndividualDto> Handle(UpdateIndividualCommand request, CancellationToken cancellationToken)
    {
        var individual = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (individual == null)
            throw new KeyNotFoundException($"Individual with ID {request.Id} not found");

        request.Dto.Adapt(individual);
        individual.UpdatedAt = DateTime.UtcNow;
        individual.UpdatedBy = "system";

        await _repository.UpdateAsync(individual, cancellationToken);
        return individual.Adapt<IndividualDto>();
    }
}
