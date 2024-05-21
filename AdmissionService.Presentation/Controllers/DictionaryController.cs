using AdmissionService.Application.Features.Queries.GetFaculties;
using AdmissionService.Application.Features.Queries.GetPrograms;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionService.Presentation.Controllers;

[ApiController, Route("api")]
public class DictionaryController : ControllerBase
{
    private readonly IMediator _mediator;

    public DictionaryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, Route("faculties")]
    public async Task<IActionResult> GetFaculties()
    {
        return Ok(await _mediator.Send(new GetFacultiesCommand()));
    }
    
    [HttpGet, Route("programs")]
    public async Task<IActionResult> GetPrograms()
    {
        return Ok(await _mediator.Send(new GetProgramsCommand()));
    }
}