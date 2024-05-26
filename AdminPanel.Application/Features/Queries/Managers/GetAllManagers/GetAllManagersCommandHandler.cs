using AdminPanel.Application.Contracts.Persistence;
using AutoMapper;
using Common.Models.Models.Dtos;
using MediatR;

namespace AdminPanel.Application.Features.Queries.Managers.GetAllManagers;

public class GetAllManagersCommandHandler : IRequestHandler<GetAllManagersCommand, Unit>
{
    private readonly IBaseManagerRepository _repository;
    private readonly IManagerRepository _manager;
    private readonly IMapper _mapper;

    public GetAllManagersCommandHandler(IBaseManagerRepository repository, IMapper mapper, IManagerRepository manager)
    {
        _repository = repository;
        _mapper = mapper;
        _manager = manager;
    }

    public async Task<Unit> Handle(GetAllManagersCommand request, CancellationToken cancellationToken)
    {
        var managers = await _repository.GetAllAsync();
        
        List<ManagerDto> list = new List<ManagerDto>();
        foreach (var manager in managers)
        {
            var dto = _mapper.Map<ManagerDto>(manager);

            if (await _repository.CheckIfManager(manager))
            {
                var facultyId = await _manager.GetFaculty(manager.Id);
                dto.FacultyId = facultyId;
                
            }

            list.Add(dto);
        }
        
        return Unit.Value;
    }
}