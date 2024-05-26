using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Domain.Entities;
using Common.Models.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AdminPanel.Application.Features.Commands.Account.EditPassword;

public class EditPasswordCommandHandler : IRequestHandler<EditPasswordCommand, Unit>
{
    private readonly UserManager<BaseManager> _manager;
    private readonly PubSubSender _pubSub;

    public EditPasswordCommandHandler(UserManager<BaseManager> manager, PubSubSender pubSub)
    {
        _manager = manager;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(EditPasswordCommand request, CancellationToken cancellationToken)
    {
        var manager = await _manager.Users.FirstOrDefaultAsync(x => x.UserId == request.ManagerId);

        if (!await _manager.CheckPasswordAsync(manager!, request.OldPassword))
            throw new BadRequest("Invalid password");

        await _manager.ChangePasswordAsync(manager, request.OldPassword, request.NewPassword);

        await _pubSub.EditPassword(manager.UserId, request.OldPassword, request.NewPassword);

        return Unit.Value;
    }
}