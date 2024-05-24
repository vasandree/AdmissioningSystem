using AdmissionService.Application.Dtos.Responses;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetMyAdmissionById;

public record GetAdmissionByIdCommand(Guid UserId, Guid AdmissionId) : IRequest<AdmissionDto>;