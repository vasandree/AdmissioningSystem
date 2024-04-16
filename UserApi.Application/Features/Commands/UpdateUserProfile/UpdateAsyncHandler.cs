using AutoMapper;
using MediatR;
using UserApi.Application.Contracts.Persistence;
using UserApi.Application.Dtos.Requests;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Commands.UpdateUserProfile;

public class UpdateUserInfoHandler : IRequestHandler<UpdateUserProfile, Unit>
{

    private readonly IUserRepository _repository;

    public UpdateUserInfoHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(UpdateUserProfile request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmail(request.Email);
        if (user == null) throw new Exception("No such user");
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
