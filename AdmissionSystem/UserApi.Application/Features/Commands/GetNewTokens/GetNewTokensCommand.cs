using MediatR;
using UserApi.Application.Dtos.Responses;

namespace UserApi.Application.Features.Commands.GetNewTokens;

public record GetNewTokensCommand(string Email) :  IRequest<TokenResponseDto>;