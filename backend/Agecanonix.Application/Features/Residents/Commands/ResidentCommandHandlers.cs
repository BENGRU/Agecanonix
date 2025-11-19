using Agecanonix.Application.DTOs.Resident;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Residents.Commands;

public class CreateResidentCommandHandler : IRequestHandler<CreateResidentCommand, ResidentDto>
{
    private readonly IRepository<Resident> _repository;
    private readonly IMapper _mapper;

    public CreateResidentCommandHandler(IRepository<Resident> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResidentDto> Handle(CreateResidentCommand request, CancellationToken cancellationToken)
    {
        var resident = _mapper.Map<Resident>(request.Dto);
        resident.Id = Guid.NewGuid();
        resident.CreatedAt = DateTime.UtcNow;
        resident.CreatedBy = "system";

        var created = await _repository.AddAsync(resident);
        return _mapper.Map<ResidentDto>(created);
    }
}

public class UpdateResidentCommandHandler : IRequestHandler<UpdateResidentCommand, ResidentDto>
{
    private readonly IRepository<Resident> _repository;
    private readonly IMapper _mapper;

    public UpdateResidentCommandHandler(IRepository<Resident> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResidentDto> Handle(UpdateResidentCommand request, CancellationToken cancellationToken)
    {
        var resident = await _repository.GetByIdAsync(request.Id);
        if (resident == null)
            throw new KeyNotFoundException($"Resident with ID {request.Id} not found");

        _mapper.Map(request.Dto, resident);
        resident.UpdatedAt = DateTime.UtcNow;
        resident.UpdatedBy = "system";

        await _repository.UpdateAsync(resident);
        return _mapper.Map<ResidentDto>(resident);
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
