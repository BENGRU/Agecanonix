using Agecanonix.Application.DTOs.Individual;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Individuals.Commands;

public class CreateIndividualCommandHandler : IRequestHandler<CreateIndividualCommand, IndividualDto>
{
    private readonly IRepository<Individual> _repository;

    public CreateIndividualCommandHandler(IRepository<Individual> repository)
    {
        _repository = repository;
    }

    public async Task<IndividualDto> Handle(CreateIndividualCommand request, CancellationToken cancellationToken)
    {
        var individual = request.Dto.Adapt<Individual>();
        individual.Id = Guid.NewGuid();
        individual.CreatedAt = DateTime.UtcNow;
        individual.CreatedBy = "system";

        var created = await _repository.AddAsync(individual, cancellationToken);
        return created.Adapt<IndividualDto>();
    }
}
