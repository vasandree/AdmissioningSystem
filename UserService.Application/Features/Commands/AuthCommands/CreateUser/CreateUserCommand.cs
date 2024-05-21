using MediatR;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Commands.AuthCommands.CreateUser;

public record CreateUserCommand(RegisterUserDto NewUser) : IRequest<TokenResponseDto>;