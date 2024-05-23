using Common.Models.Models.Dtos.PagedDtos;
using MediatR;

namespace AdmissionService.Application.Features.Queries.GetFaculties;

public record GetFacultiesCommand() : IRequest<FacultiesPagedDto>;