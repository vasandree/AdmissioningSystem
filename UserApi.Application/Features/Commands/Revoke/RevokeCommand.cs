using MediatR;

namespace UserApi.Application.Features.Commands.Revoke;

public record RevokeCommand(string RefreshToken) : IRequest<Unit>;