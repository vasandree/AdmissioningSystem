using MediatR;

namespace AdminPanel.Application.Features.Commands.Account.EditPassword;

public record EditPasswordCommand(Guid ManagerId, string OldPassword, string NewPassword) : IRequest<Unit>;
