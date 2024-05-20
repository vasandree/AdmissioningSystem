using Common.Models.Dtos.PagedDtos;
using DictionaryService.Domain.Enums;
using MediatR;

namespace DictionaryService.Application.Features.Queries.GetPrograms;

public record GetProgramsCommand(
    Guid[]? Faculties,
    Language? Language,
    FormOfEducation? FormOfEducation,
    string? Code,
    string? Name,
    int Size,
    int Page) : IRequest<ProgramsPagedListDto>;