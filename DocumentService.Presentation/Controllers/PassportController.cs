using System.Security.Claims;
using DocumentService.Application.Features.Commands.UploadFile;
using DocumentService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Presentation.Controllers;

[ApiController, Route("api/documents/passport")]
public class PassportController : ControllerBase
{
    private readonly IMediator _mediator;

    public PassportController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize, HttpPost, Route("upload ")]
    public async Task<IActionResult> Upload([FromForm] IFormFile file)
    {
        return Ok(await _mediator.Send(new UploadPassportRequest( file,
            Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    
    
}