using MediatR;

namespace AdminPanel.Application.Features.Queries.UserInfo.GetUserInfo;

public record GetUserInfoCommand(Guid ApplicantId) : IRequest<Unit>;