using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using MediatR;

namespace Agecanonix.Application.Features.IndividualRelationships.Commands;

public class DeleteIndividualRelationshipCommandHandler : IRequestHandler<DeleteIndividualRelationshipCommand, bool>
{
    private readonly IRepository<IndividualRelationship> _repository;

    public DeleteIndividualRelationshipCommandHandler(IRepository<IndividualRelationship> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteIndividualRelationshipCommand request, CancellationToken cancellationToken)
    {
        var relationship = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (relationship == null)
            return false;

        await _repository.DeleteAsync(relationship, cancellationToken);
        return true;
    }
}
