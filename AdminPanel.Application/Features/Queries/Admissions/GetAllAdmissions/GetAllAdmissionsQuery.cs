using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Queries.Admissions.GetAllAdmissions;

public record GetAllAdmissionsQuery(
    Guid ManagerId,
    string? Name,
    Guid? Program,
    Guid[]? Faculties,
    AdmissionStatus? Status,
    bool NoManager,
    bool My,
    int Page,
    int Size) : IRequest<Unit>;