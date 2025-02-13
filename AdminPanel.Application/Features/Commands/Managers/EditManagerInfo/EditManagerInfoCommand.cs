using MediatR;

namespace AdminPanel.Application.Features.Commands.Managers.EditManagerInfo;

public record EditManagerInfoCommand(Guid ManagerId, Guid ManagerToUpdate, string FullName, string Email) : IRequest<Unit>;