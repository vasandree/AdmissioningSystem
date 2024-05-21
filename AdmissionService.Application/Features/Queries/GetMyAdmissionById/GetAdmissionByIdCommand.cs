using AdmissionService.Application.Dtos.Requests;
using AdmissionService.Application.Dtos.Responses;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetMyAdmissionById;

public record GetAdmissionByIdCommand(Guid UserId, AdmissionRequestDto AdmissionRequestDto) : IRequest<AdmissionDto>;