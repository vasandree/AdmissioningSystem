using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.DeleteAdmission;

public record DeleteAdmissionCommand(Guid AdmissionId, Guid ManagerId) : IRequest<Unit>;