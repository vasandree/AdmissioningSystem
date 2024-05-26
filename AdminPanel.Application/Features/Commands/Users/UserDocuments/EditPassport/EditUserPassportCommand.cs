using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.UserDocuments.EditPassport;

public record EditUserPassportCommand(
    Guid ManagerId,
    Guid UserId,
    string SeriesAndNumber,
    string IssuedBy,
    DateTime DateOfBirth,
    DateTime IssueDate) : IRequest<Unit>;