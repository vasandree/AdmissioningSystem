using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.TakeAdmission;

public record TakeAdmissionCommand(Guid AdmissionId, Guid ManagerId) : IRequest<Unit>;