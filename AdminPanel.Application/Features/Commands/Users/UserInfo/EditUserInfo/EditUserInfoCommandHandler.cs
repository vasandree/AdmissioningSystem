using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using Common.Models.Exceptions;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.UserInfo.EditUserInfo;

public class EditUserInfoCommandHandler : IRequestHandler<EditUserInfoCommand, Unit>
{
    private readonly RpcRequestSender _rpc;
    private readonly PubSubSender _pubSub;
    private readonly IManagerRepository _manager;
    private readonly IBaseManagerRepository _repository;

    public EditUserInfoCommandHandler(RpcRequestSender rpc, PubSubSender pubSub, IManagerRepository manager,
        IBaseManagerRepository repository)
    {
        _rpc = rpc;
        _pubSub = pubSub;
        _manager = manager;
        _repository = repository;
    }

    public async Task<Unit> Handle(EditUserInfoCommand request, CancellationToken cancellationToken)
    {
        var manager = await _repository.GetById(request.ManagerId);

        if (manager.Id == request.UserId)
            throw new BadRequest("You are not allowed to edit your info as manager");
        

        if (await _repository.CheckIfManager(manager))
        {
            if (await _rpc.CheckManagersApplicant(request.ManagerId, request.UserId)) 
                throw new Forbidden("You are not a manager of this applicant");

        }

        await _pubSub.UpdateUserInfo(request.UserId, request.FullName, request.Gender, request.Nationality,
            request.BirthDate);

        return Unit.Value;
    }
}