using AdmissionService.Application.Dtos.Responses;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetAllMyAdmissions;

public record GetAllMyAdmissionsCommand(Guid UserId) : IRequest<List<AdmissionListDto>>;