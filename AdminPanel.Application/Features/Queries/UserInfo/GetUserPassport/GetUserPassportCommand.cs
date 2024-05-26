using MediatR;

namespace AdminPanel.Application.Features.Queries.UserInfo.GetUserPassport;

public record GetUserPassportCommand(Guid UserId) : IRequest<Unit>;