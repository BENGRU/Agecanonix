using Agecanonix.Application.DTOs.Individual;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Individuals.Queries;

public class GetIndividualByIdQueryHandler : IRequestHandler<GetIndividualByIdQuery, IndividualDto?>
{
    private readonly IRepository<Individual> _repository;

    public GetIndividualByIdQueryHandler(IRepository<Individual> repository)
    {
        _repository = repository;
    }

    public async Task<IndividualDto?> Handle(GetIndividualByIdQuery request, CancellationToken cancellationToken)
    {
        var individual = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return individual?.Adapt<IndividualDto>();
    }
}
