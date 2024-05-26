using MediatR;

namespace AdminPanel.Application.Features.Queries.Admissions.GetUserAdmissions;

public record GetApplicantAdmissionsCommand(Guid ManagerId, Guid ApplicantId) : IRequest<Unit>;