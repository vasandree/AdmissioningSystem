using MediatR;
using UserApi.Application.Dtos.Requests;

namespace UserApi.Application.Features.Commands.UpdateUserProfile;

public record UpdateUserProfile(string Email, EditProfileDto NewUserInfo) : IRequest;