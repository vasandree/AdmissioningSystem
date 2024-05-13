using MediatR;

namespace UserService.Application.Features.Commands.AuthCommands.RevokeAllTokens;

public record RevokeAllTokensCommand(Guid UserId) : IRequest<Unit>;