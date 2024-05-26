using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.EditPriority;

public record EditPriorityCommand(Guid AdmissionId, Guid ManagerId, int NewPriority) : IRequest<Unit>;