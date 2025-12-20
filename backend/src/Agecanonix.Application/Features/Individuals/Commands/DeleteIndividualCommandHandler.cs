using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using MediatR;

namespace Agecanonix.Application.Features.Individuals.Commands;

public class DeleteIndividualCommandHandler : IRequestHandler<DeleteIndividualCommand, bool>
{
    private readonly IRepository<Individual> _repository;

    public DeleteIndividualCommandHandler(IRepository<Individual> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteIndividualCommand request, CancellationToken cancellationToken)
    {
        var individual = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (individual == null)
            return false;

        await _repository.DeleteAsync(individual, cancellationToken);
        return true;
    }
}
