using System.ComponentModel.DataAnnotations;
using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DictionaryService.Application.Features.Commands.CheckImportStatus;
using DictionaryService.Application.Features.Commands.ImportDictionaries;
using DictionaryService.Domain.Enums;
using DictionaryService.Persistence.Helpers;
using EasyNetQ;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryService.Presentation.Controllers;

[Route("api/dictionary")]
public class DictionaryController : ControllerBase
{
    private readonly IMediator _meditor;
    private readonly RpcRequestSender _rpc;

    public DictionaryController(IMediator meditor, IBus bus, RpcRequestSender rpc)
    {
        _meditor = meditor;
        _rpc = rpc;
    }

    [HttpPost]
    [Authorize]
    [Route("import")]
    public async Task<IActionResult> ImportDictionary([FromQuery] DictionaryType dictionaryType = DictionaryType.All)
    {
        var isAdmin = await _rpc.CheckIfAdmin(Guid.Parse(User.FindFirst("UserId")!.Value!));
        
        if (isAdmin.IsInRole)
        {
            return Ok(await _meditor.Send(new ImportDictionariesCommand(dictionaryType)));
        }

        throw new Forbidden("You don't have permission to perform this action.");
    }

    [HttpPost]
    [Authorize]
    [Route("status")]
    public async Task<IActionResult> CheckImportStatus([Required] [FromQuery] Guid requestId)
    {
        var isAdmin = await _rpc.CheckIfAdmin(Guid.Parse(User.FindFirst("UserId")!.Value!));

        if (isAdmin.IsInRole)
        {
            return Ok(await _meditor.Send(new CheckImportStatusCommand(requestId)));
        }

        throw new Forbidden("You don't have permission to perform this action.");
    }
}