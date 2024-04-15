using MediatR;
using UserApi.Application.Dtos.Requests;
using UserApi.Application.Dtos.Responses;

namespace UserApi.Application.Features.Commands.CreateUser;

public record CreateUser(RegisterUserDto NewUser) : IRequest<TokenResponseDto>;