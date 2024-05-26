using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.UserInfo.EditUserInfo;

public record EditUserInfoCommand(
    Guid ManagerId,
    Guid UserId,
    string FullName,
    DateTime BirthDate,
    Gender Gender,
    string Nationality) : IRequest<Unit>;