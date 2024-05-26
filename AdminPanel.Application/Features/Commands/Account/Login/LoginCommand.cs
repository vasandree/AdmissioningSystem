using MediatR;
using UserService.Application.Dtos.Requests;

namespace AdminPanel.Application.Features.Commands.Account.Login;

public record LoginCommand(LoginUserDto LoginUserDto): IRequest<Unit>;