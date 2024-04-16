using MediatR;
using UserApi.Application.Dtos.Requests;
using UserApi.Application.Dtos.Responses;

namespace UserApi.Application.Features.Commands.GetNewTokens;

public record GetNewTokensCommand(RefreshTokenDto RefreshTokenDto) :  IRequest<TokenResponseDto>;