using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Domain.Entities;
using Common.Models.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Application.Features.Commands.Managers.DeleteManager;

public class DeleteManagerCommandHandler : IRequestHandler<DeleteManagerCommand, Unit>
{
    private readonly UserManager<BaseManager> _userManager;
    private readonly IBaseManagerRepository _repository;
    private readonly PubSubSender _pubSub;

    public DeleteManagerCommandHandler(UserManager<BaseManager> userManager, PubSubSender pubSub,
        IBaseManagerRepository repository)
    {
        _userManager = userManager;
        _pubSub = pubSub;
        _repository = repository;
    }

    public async Task<Unit> Handle(DeleteManagerCommand request, CancellationToken cancellationToken)
    {
        if (request.ManagerId == request.ManagerToDeleteId)
            throw new BadRequest("You cannot delete yourself");

        if (!await _repository.CheckExistence(request.ManagerToDeleteId))
            throw new BadRequest("Provided manager does not exist");

        var manager = await _repository.GetById(request.ManagerToDeleteId);

        var role = await _userManager.GetRolesAsync(manager);
        
        await _repository.DeleteAsync(manager);
        
        await _pubSub.UpdateRole(manager.UserId, role[0]);

        return Unit.Value;
    }
}