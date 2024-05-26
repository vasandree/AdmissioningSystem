using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.Rpc;
using AutoMapper;
using Common.Models.Models.Dtos;
using MediatR;

namespace AdminPanel.Application.Features.Queries.Managers.GetAllManagers;

public class GetAllManagersCommandHandler : IRequestHandler<GetAllManagersCommand, List<ManagerDto>>
{
    private readonly IBaseManagerRepository _repository;
    private readonly IManagerRepository _manager;
    private readonly RpcRequestSender _rpc;

    public GetAllManagersCommandHandler(IBaseManagerRepository repository,  IManagerRepository manager,
        RpcRequestSender rpc)
    {
        _repository = repository;
        _manager = manager;
        _rpc = rpc;
    }

    public async Task<List<ManagerDto>> Handle(GetAllManagersCommand request, CancellationToken cancellationToken)
    {
        var managers = await _repository.GetAllAsync();

        List<ManagerDto> list = new List<ManagerDto>();
        foreach (var manager in managers)
        {
            var dto = new ManagerDto
            {
                Id = manager.Id,
                FullName = manager.FullName,
                Email = manager!.Email,
            };

            if (await _repository.CheckIfManager(manager))
            {
                var facultyId = await _manager.GetFaculty(manager.Id);
                dto.Faculty = await _rpc.GetFaculty(facultyId);
            }

            list.Add(dto);
        }

        return list;
    }
}