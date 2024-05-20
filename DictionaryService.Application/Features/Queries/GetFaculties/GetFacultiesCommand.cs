using Common.Models.Dtos.PagedDtos;
using MediatR;

namespace DictionaryService.Application.Features.Queries.GetFaculties;

public record GetFacultiesCommand(int Size, int Page) : IRequest<FacultiesPagedDto>;