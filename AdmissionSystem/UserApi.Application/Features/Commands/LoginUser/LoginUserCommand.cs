using MediatR;
using UserApi.Application.Dtos.Requests;
using UserApi.Application.Dtos.Responses;

namespace UserApi.Application.Features.Commands.LoginUser;

public record LoginUserCommand(LoginUserDto LoginUserDto) : IRequest<TokenResponseDto>;