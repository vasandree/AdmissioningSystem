using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.GiveAdmission;

public record GiveAdmissionCommand(Guid ManagerId, Guid ManagerToGive, Guid AdmissionId):IRequest<Unit>;