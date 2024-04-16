using MediatR;

namespace UserApi.Application.Features.Commands.Revoke;

public record RevokeCommand(string Email) : IRequest<Unit>;