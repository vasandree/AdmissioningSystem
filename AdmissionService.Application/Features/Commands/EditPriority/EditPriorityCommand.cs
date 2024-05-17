using AdmissionService.Application.Dtos.Requests;
using MediatR;

namespace AdmissionService.Application.Features.Commands.EditPriority;

public record EditPriorityCommand(Guid UserId, ChangeAdmissionPriorityDto ChangeAdmissionPriorityDto) : IRequest<Unit>;