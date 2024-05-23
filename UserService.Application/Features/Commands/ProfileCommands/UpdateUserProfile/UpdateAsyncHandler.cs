using Common.Models.Exceptions;
using MediatR;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;
using UserService.Application.Dtos.Requests;

namespace UserService.Application.Features.Commands.ProfileCommands.UpdateUserProfile;

public class UpdateUserInfoHandler : IRequestHandler<UpdateUserProfile, Unit>
{

    private readonly IUserRepository _repository;

    public UpdateUserInfoHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateUserProfile request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById(request.Id);
        if (user == null) throw new BadRequest("No such user");
        
        var newProfileInfo = PutNewValues(request.NewUserInfo, user);
        await _repository.UpdateAsync(newProfileInfo);
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
