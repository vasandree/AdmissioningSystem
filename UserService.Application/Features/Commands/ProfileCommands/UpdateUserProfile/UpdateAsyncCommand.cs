using MediatR;
using UserService.Application.Dtos.Requests;

namespace UserService.Application.Features.Commands.ProfileCommands.UpdateUserProfile;

public record UpdateUserProfile(Guid Id, EditProfileDto NewUserInfo) : IRequest<Unit>;