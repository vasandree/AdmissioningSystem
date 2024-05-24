using AdmissionService.Application.Dtos.Requests;
using MediatR;

namespace AdmissionService.Application.Features.Commands.DeleteAdmission;

public record DeleteAdmissionCommand(Guid UserId, Guid AdmissionId) : IRequest<Unit>;