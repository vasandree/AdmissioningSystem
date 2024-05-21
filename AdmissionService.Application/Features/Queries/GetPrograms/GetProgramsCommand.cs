using Common.Models.Dtos.PagedDtos;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetPrograms;

public record GetProgramsCommand() : IRequest<ProgramsPagedListDto>;