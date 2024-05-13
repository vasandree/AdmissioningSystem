using MediatR;
using UserService.Application.Dtos.Requests;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Commands.AuthCommands.GetNewTokens;

public record GetNewTokensCommand(RefreshTokenDto RefreshTokenDto, Guid UserId) :  IRequest<TokenResponseDto>;