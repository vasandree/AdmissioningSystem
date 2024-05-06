using DictionaryService.Application.Features.Commands.ImportDictionaries;
using DictionaryService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryService.Presentation.Controllers;

[Route("api/dictionary")]
public class DictionaryController : ControllerBase
{
    private readonly IMediator _meditor;

    public DictionaryController(IMediator meditor)
    {
        _meditor = meditor;
    }

    [HttpPost]
    [Authorize]
    [Route("import")]
    public async Task<IActionResult> ImportDictionary(DictionaryType dictionaryType)
    {
        return Ok(await _meditor.Send( new ImportDictionariesCommand(dictionaryType)));
    }

    /*[HttpPost]
    [Authorize]
    [Route("status")]
    public async Task<IActionResult> CheckImportStatus(DictionaryType? dictionaryType)
    {
        return Ok(await _meditor.Send(new CheckImportStatusCommand(dictionaryType)));
    }*/
}