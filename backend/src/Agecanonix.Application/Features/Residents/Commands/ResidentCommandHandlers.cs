using Agecanonix.Application.DTOs.Resident;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Mapster;
using MediatR;

namespace Agecanonix.Application.Features.Residents.Commands;

public class CreateResidentCommandHandler : IRequestHandler<CreateResidentCommand, ResidentDto>
{
    private readonly IRepository<Resident> _repository;

    public CreateResidentCommandHandler(IRepository<Resident> repository)
    {
        _repository = repository;
    }

    public async Task<ResidentDto> Handle(CreateResidentCommand request, CancellationToken cancellationToken)
    {
        var resident = request.Dto.Adapt<Resident>();
        resident.Id = Guid.NewGuid();
        resident.CreatedAt = DateTime.UtcNow;
        resident.CreatedBy = "system";

        var created = await _repository.AddAsync(resident);
        return created.Adapt<ResidentDto>();
    }
}

public class UpdateResidentCommandHandler : IRequestHandler<UpdateResidentCommand, ResidentDto>
{
    private readonly IRepository<Resident> _repository;

    public UpdateResidentCommandHandler(IRepository<Resident> repository)
    {
        _repository = repository;
    }

    public async Task<ResidentDto> Handle(UpdateResidentCommand request, CancellationToken cancellationToken)
    {
        var resident = await _repository.GetByIdAsync(request.Id);
        if (resident == null)
            throw new KeyNotFoundException($"Resident with ID {request.Id} not found");

        request.Dto.Adapt(resident);
        resident.UpdatedAt = DateTime.UtcNow;
        resident.UpdatedBy = "system";

        await _repository.UpdateAsync(resident);
        return resident.Adapt<ResidentDto>();
    }
}

public class DeleteResidentCommandHandler : IRequestHandler<DeleteResidentCommand, bool>
{
    private readonly IRepository<Resident> _repository;

    public DeleteResidentCommandHandler(IRepository<Resident> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteResidentCommand request, CancellationToken cancellationToken)
    {
        var resident = await _repository.GetByIdAsync(request.Id);
        if (resident == null)
            return false;

        await _repository.DeleteAsync(resident);
        return true;
    }
}
