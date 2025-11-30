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
        var resident = request.Dto.Adapt<Individual>();
        resident.Id = Guid.NewGuid();
        resident.CreatedAt = DateTime.UtcNow;
        resident.CreatedBy = "system";

        var created = await _repository.AddAsync(resident);
        return created.Adapt<IndividualDto>();
    }
}

public class UpdateIndividualCommandHandler : IRequestHandler<UpdateIndividualCommand, IndividualDto>
{
    private readonly IRepository<Individual> _repository;

    public UpdateIndividualCommandHandler(IRepository<Individual> repository)
    {
        _repository = repository;
    }

    public async Task<IndividualDto> Handle(UpdateIndividualCommand request, CancellationToken cancellationToken)
    {
        var resident = await _repository.GetByIdAsync(request.Id);
        if (resident == null)
            throw new KeyNotFoundException($"Individual with ID {request.Id} not found");

        request.Dto.Adapt(resident);
        resident.UpdatedAt = DateTime.UtcNow;
        resident.UpdatedBy = "system";

        await _repository.UpdateAsync(resident);
        return resident.Adapt<IndividualDto>();
    }
}

public class DeleteIndividualCommandHandler : IRequestHandler<DeleteIndividualCommand, bool>
{
    private readonly IRepository<Individual> _repository;

    public DeleteIndividualCommandHandler(IRepository<Individual> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteIndividualCommand request, CancellationToken cancellationToken)
    {
        var resident = await _repository.GetByIdAsync(request.Id);
        if (resident == null)
            return false;

        await _repository.DeleteAsync(resident);
        return true;
    }
}
