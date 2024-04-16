using MediatR;
using UserApi.Application.Dtos.Requests;

namespace UserApi.Application.Features.Commands.ForgetPassword;

public record ForgetPasswordCommand(string Email, ForgetPasswordDto ForgetPassword): IRequest<Unit>;