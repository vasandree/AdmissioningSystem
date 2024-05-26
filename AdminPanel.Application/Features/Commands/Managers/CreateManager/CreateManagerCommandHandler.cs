using AdminPanel.Application.Contracts.Persistence;
using AdminPanel.Application.ServiceBus.PubSub.Sender;
using AdminPanel.Application.ServiceBus.Rpc;
using AdminPanel.Domain.Entities;
using Common.Models.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Application.Features.Commands.Managers.CreateManager;

public class CreateManagerCommandHandler : IRequestHandler<CreateManagerCommand, Unit>
{
    private readonly IBaseManagerRepository _repository;
    private readonly IManagerRepository _manager;
    private readonly IHeadManagerRepository _headManager;
    private readonly UserManager<BaseManager> _userManager;
    private readonly PubSubSender _pubSub;
    private readonly RpcRequestSender _rpc;

    public CreateManagerCommandHandler(IBaseManagerRepository repository, RpcRequestSender rpc,
        UserManager<BaseManager> userManager, PubSubSender pubSub, IManagerRepository manager,
        IHeadManagerRepository headManager)
    {
        _repository = repository;
        _rpc = rpc;
        _userManager = userManager;
        _pubSub = pubSub;
        _manager = manager;
        _headManager = headManager;
    }

    public async Task<Unit> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.CheckExistence(request.UserId))
            throw new BadRequest("Provided user already has role");

        var existingUser = await _rpc.GetUser(request.UserId);

        var user = new BaseManager
        {
            UserId = existingUser.UserId,
            UserName = existingUser.Email,
            Email = existingUser.Email,
            PasswordHash = existingUser.Password
        };

        if (request.FacultyId != null)
        {
            if (!await _rpc.CheckFaculty((Guid)request.FacultyId))
                throw new NotFound("Provided faculty does not exist");


            await _manager.CreateAsync(new Manager()
            {
                ManagerId = Guid.NewGuid(),
                MainId = user.Id,
                BaseManager = user
            });

            _pubSub.UpdateRole(user.UserId, "Manager", request.FacultyId);
            
            _pubSub.SendEmail(user.Email, "Manager", request.FacultyId);
        }


        await _userManager.CreateAsync(user);
        await _userManager.AddToRoleAsync(user, "HeadManager");


        await _headManager.CreateAsync(new HeadManager
        {
            HeadManagerId = Guid.NewGuid(),
            MainId = user.Id,
            BaseManager = user
        });

        _pubSub.UpdateRole(user.UserId, "HeadManager");

        return Unit.Value;
    }
}