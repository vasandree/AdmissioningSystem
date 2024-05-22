using MediatR;
using UserService.Application.Dtos.Requests;

namespace UserService.Application.Features.Commands.AuthCommands.Revoke;

public record RevokeCommand(Guid UserId, RefreshTokenDto RevokeTokenDto) : IRequest<Unit>;