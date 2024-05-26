using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using AdminPanel.Domain.Entities;
using Common.Models.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Application.Features.Commands.Managers.EditManagerInfo;

public class EditManagerInfoCommandHandler : IRequestHandler<EditManagerInfoCommand, Unit>
{
    private readonly IBaseManagerRepository _repository;
    private readonly UserManager<BaseManager> _userManager;
    private readonly IManagerRepository _manager;
    private readonly IHeadManagerRepository _headManager;
    private readonly RpcRequestSender _rpc;
    private readonly PubSubSender _pubSub;


    public EditManagerInfoCommandHandler(IBaseManagerRepository repository, UserManager<BaseManager> userManager, IManagerRepository manager, IHeadManagerRepository headManager, RpcRequestSender rpc, PubSubSender pubSub)
    {
        _repository = repository;
        _userManager = userManager;
        _manager = manager;
        _headManager = headManager;
        _rpc = rpc;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(EditManagerInfoCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.CheckExistence(request.ManagerToUpdate))
            throw new BadRequest("Provided manager does not exist");

        var manager = await _repository.GetById(request.ManagerToUpdate);

        if (manager.FullName != request.FullName)
        {
            manager.FullName = request.FullName;
            await _repository.UpdateAsync(manager);
            _pubSub.UpdateUserInfo(request.ManagerToUpdate, manager.FullName);
        }

        return Unit.Value;
    }
}