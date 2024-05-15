using DocumentService.Application.Dtos.Requests;
using DocumentService.Application.Features.Commands.PassportInfo.AddPassportInfo;
using DocumentService.Application.Features.Commands.PassportInfo.DeletePassportInfo;
using DocumentService.Application.Features.Commands.PassportInfo.EditPassportInfo;
using DocumentService.Application.Features.Queries.GetPassportInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Presentation.Controllers;

[ApiController, Route("api/passport_info")]
public class PassportInfoController : ControllerBase
{
    private readonly IMediator _mediator;

    public PassportInfoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetPassportInfo()
    {
        return Ok(await _mediator.Send(new GetPassportInfoCommand(Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> AddPassportInfo([FromBody] PassportInfoRequest passportInfoRequest)
    {
        return Ok(await _mediator.Send(new AddPassportInfoCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            passportInfoRequest)));
    }

    [HttpPut, Authorize]
    public async Task<IActionResult> EditPassportInfo([FromBody] PassportInfoRequest passportInfoRequest)
    {
        return Ok(await _mediator.Send(new EditPassportInfoCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            passportInfoRequest)));
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> DeletePassportInfo()
    {
        return Ok(await _mediator.Send(new DeletePassportInfoCommand(Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }
}