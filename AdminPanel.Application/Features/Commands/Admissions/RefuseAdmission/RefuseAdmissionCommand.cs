using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.RefuseAdmission;

public record RefuseAdmissionCommand(Guid AdmissionId, Guid ManagerId) : IRequest<Unit>;