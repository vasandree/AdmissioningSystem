using MediatR;
using UserService.Application.Dtos.Requests;

namespace UserService.Application.Features.Commands.ProfileCommands.ForgetPassword;

public record ForgetPasswordCommand(Guid Id, ForgetPasswordDto ForgetPassword): IRequest<Unit>;