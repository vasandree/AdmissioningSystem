using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Admissions.EditStatus;

public record EditAdmissionStatusCommand(Guid AdmissionId, Guid ManagerId, AdmissionStatus Status) : IRequest<Unit>;