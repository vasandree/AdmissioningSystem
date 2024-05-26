using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.Rpc;
using AutoMapper;
using Common.Models.Exceptions;
using Common.Models.Models.Dtos;
using MediatR;

namespace AdminPanel.Application.Features.Queries.GetManagerProfile;

public class GetManagerProfileCommandHandler : IRequestHandler<GetManagerProfileCommand, ManagerDto>
{
    private readonly IBaseManagerRepository _repository;
    private readonly IManagerRepository _manager;
    private readonly IMapper _mapper;
    private readonly RpcRequestSender _rpc;

    public GetManagerProfileCommandHandler(IBaseManagerRepository repository, IManagerRepository manager, IMapper mapper, RpcRequestSender rpc)
    {
        _repository = repository;
        _manager = manager;
        _mapper = mapper;
        _rpc = rpc;
    }

    public async Task<ManagerDto> Handle(GetManagerProfileCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.CheckExistence(request.ManagerId))
            throw new NotFound("Manager not found");

        var manager = await _repository.GetById(request.ManagerId);

        var dto = new ManagerDto
        {
            Id = manager.Id,
            FullName = manager.FullName,
            Email = manager.Email!,
        };

        if (await _repository.CheckIfManager(manager))
        {
            dto.Faculty = await _rpc.GetFaculty(await _manager.GetFaculty(manager.Id));
        }

        return dto;
    }
}