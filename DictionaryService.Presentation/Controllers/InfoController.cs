using DictionaryService.Application.Features.Queries.GetDocumentType;
using DictionaryService.Application.Features.Queries.GetEducationLevels;
using DictionaryService.Application.Features.Queries.GetFaculties;
using DictionaryService.Application.Features.Queries.GetPrograms;
using DictionaryService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryService.Presentation.Controllers;

[ApiController, Route("api/dictionary")]
public class InfoController : ControllerBase
{
    private readonly IMediator _mediator;
    private int _size;

    public InfoController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _size = configuration.GetValue<int>("DefaultPageSize");
    }

    [HttpGet, Route("education_levels")]
    public async Task<IActionResult> GetAllEducationLevels()
    {
        return Ok(await _mediator.Send(new GetEducationLevelsCommand()));
    }

    [HttpGet, Route("document_types")]
    public async Task<IActionResult> GetAllDocumentTypes()
    {
        return Ok(await _mediator.Send(new GetDocumentTypeCommand()));
    }

    [HttpGet, Route("faculties")]
    public async Task<IActionResult> GetAllFaculties([FromQuery] int? size = 10, [FromQuery] int? page = 1)
    {
        return Ok(await _mediator.Send(new GetFacultiesCommand(size ?? _size, page ?? 1)));
    }

    [HttpGet, Route("programs")]
    public async Task<IActionResult> GetPrograms(
        [FromQuery] Guid[]? faculties,
        [FromQuery] Language? language,
        [FromQuery] FormOfEducation? formOfEducation,
        [FromQuery] string? code,
        [FromQuery] string? name,
        [FromQuery] int? size = 10,
        [FromQuery] int? page = 1)
    {
        return Ok(await _mediator.Send(new GetProgramsCommand(faculties, language, formOfEducation, code, name,
            size ?? _size, page ?? 1)));
    }
}