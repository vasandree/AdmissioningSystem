using AdmissionService.Application.Dtos.Responses;
using Common.Models.Models;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetAllMyAdmissions;

public record GetAllMyAdmissionsCommand(Guid UserId) : IRequest<List<AdmissionDto>>;