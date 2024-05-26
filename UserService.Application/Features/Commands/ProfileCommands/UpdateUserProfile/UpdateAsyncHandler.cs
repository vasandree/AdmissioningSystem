using Common.Models.Exceptions;
using MediatR;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;
using UserService.Application.Dtos.Requests;
using UserService.Application.ServiceBus.PubSub.Sender;
using UserService.Application.ServiceBus.RPC.RpcRequestSender;

namespace UserService.Application.Features.Commands.ProfileCommands.UpdateUserProfile;

public class UpdateUserInfoHandler : IRequestHandler<UpdateUserProfile, Unit>
{

    private readonly IUserRepository _repository;
    private readonly RpcRequestSender _rpc;
    private readonly PubSubSender _pubSub;

    public UpdateUserInfoHandler(IUserRepository repository, RpcRequestSender rpc, PubSubSender pubSub)
    {
        _repository = repository;
        _rpc = rpc;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(UpdateUserProfile request, CancellationToken cancellationToken)
    {
        
        var user = await _repository.GetById(request.Id);
        if (user == null) throw new BadRequest("No such user");

        if (await _rpc.CheckStatusClosedByUserId(request.Id))
            throw new BadRequest("One of your admissions is closed");
        
        var newProfileInfo = PutNewValues(request.NewUserInfo, user);
        await _repository.UpdateAsync(newProfileInfo);
        
        _pubSub.UpdateStatus(request.Id);
        
        return Unit.Value;
    }

    private ApplicationUser PutNewValues(EditProfileDto profileDto, ApplicationUser user)
    {
        user.BirthDate = profileDto.BirthDate;
        user.Gender = profileDto.Gender;
        user.Nationality = profileDto.Nationality;
        user.FullName = profileDto.FullName;
        return user;
    }
}
