using AdmissionService.Application.Dtos.Requests;
using MediatR;

namespace AdmissionService.Application.Features.Commands.CreateNewAdmission;

public record CreateNewAdmissionCommand(Guid UserId, AdmissionRequest AdmissionRequest) : IRequest<Unit>;