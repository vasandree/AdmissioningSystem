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
    private readonly PubSubSender _pubSub;


    public EditManagerInfoCommandHandler(IBaseManagerRepository repository, UserManager<BaseManager> userManager, IManagerRepository manager, IHeadManagerRepository headManager, RpcRequestSender rpc, PubSubSender pubSub)
    {
        _repository = repository;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(EditManagerInfoCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.CheckExistence(request.ManagerToUpdate))
            throw new BadRequest("Provided manager does not exist");

        var manager = await _repository.GetById(request.ManagerToUpdate);

        if (manager.FullName != request.FullName || request.Email != manager.Email)
        {
            manager.FullName = request.FullName;
            manager.Email = request.Email;
            await _repository.UpdateAsync(manager);
            await _pubSub.UpdateEmailAndFullName(request.ManagerToUpdate, manager.FullName, manager.Email);
        }

        return Unit.Value;
    }
}