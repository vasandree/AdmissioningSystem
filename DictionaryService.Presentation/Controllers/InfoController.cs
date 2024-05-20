using DictionaryService.Application.Features.Queries.GetDocumentType;
using DictionaryService.Application.Features.Queries.GetEducationLevels;
using DictionaryService.Application.Features.Queries.GetFaculties;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryService.Presentation.Controllers;

[ApiController, Route("api/dictionary")]
public class InfoController : ControllerBase
{
    private readonly IMediator _mediator;

    public InfoController(IMediator mediator)
    {
        _mediator = mediator;
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
    public async Task<IActionResult> GetAllFaculties([FromQuery] int size = 10, [FromQuery]int page = 1)
    {
        return Ok(await _mediator.Send(new GetFacultiesCommand(size, page)));
    }
}