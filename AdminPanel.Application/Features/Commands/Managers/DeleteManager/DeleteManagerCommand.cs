using MediatR;

namespace AdminPanel.Application.Features.Commands.Managers.DeleteManager;

public record DeleteManagerCommand(Guid ManagerId, Guid ManagerToDeleteId) : IRequest<Unit>;