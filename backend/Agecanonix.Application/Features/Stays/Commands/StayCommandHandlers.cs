using Agecanonix.Application.DTOs.Stay;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using AutoMapper;
using MediatR;

namespace Agecanonix.Application.Features.Stays.Commands;

public class CreateStayCommandHandler : IRequestHandler<CreateStayCommand, StayDto>
{
    private readonly IRepository<Stay> _repository;
    private readonly IMapper _mapper;

    public CreateStayCommandHandler(IRepository<Stay> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StayDto> Handle(CreateStayCommand request, CancellationToken cancellationToken)
    {
        var stay = _mapper.Map<Stay>(request.Dto);
        stay.Id = Guid.NewGuid();
        stay.CreatedAt = DateTime.UtcNow;
        stay.CreatedBy = "system";

        var created = await _repository.AddAsync(stay);
        return _mapper.Map<StayDto>(created);
    }
}

public class UpdateStayCommandHandler : IRequestHandler<UpdateStayCommand, StayDto>
{
    private readonly IRepository<Stay> _repository;
    private readonly IMapper _mapper;

    public UpdateStayCommandHandler(IRepository<Stay> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<StayDto> Handle(UpdateStayCommand request, CancellationToken cancellationToken)
    {
        var stay = await _repository.GetByIdAsync(request.Id);
        if (stay == null)
            throw new KeyNotFoundException($"Stay with ID {request.Id} not found");

        _mapper.Map(request.Dto, stay);
        stay.UpdatedAt = DateTime.UtcNow;
        stay.UpdatedBy = "system";

        await _repository.UpdateAsync(stay);
        return _mapper.Map<StayDto>(stay);
    }
}

public class DeleteStayCommandHandler : IRequestHandler<DeleteStayCommand, bool>
{
    private readonly IRepository<Stay> _repository;

    public DeleteStayCommandHandler(IRepository<Stay> repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteStayCommand request, CancellationToken cancellationToken)
    {
        var stay = await _repository.GetByIdAsync(request.Id);
        if (stay == null)
            return false;

        await _repository.DeleteAsync(stay);
        return true;
    }
}
