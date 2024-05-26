using AdmissionService.Application.Dtos.Responses;
using Common.Models.Models;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetMyAdmissionById;

public record GetAdmissionByIdCommand(Guid UserId, Guid AdmissionId) : IRequest<AdmissionDto>;