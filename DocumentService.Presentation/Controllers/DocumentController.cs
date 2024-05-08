using DocumentService.Application.Features.Commands.DeleteDocument;
using DocumentService.Application.Features.Commands.EditDocument;
using DocumentService.Application.Features.Commands.UploadDocument;
using DocumentService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Presentation.Controllers;

[ApiController, Route("api/documents")]
public class DocumentController : ControllerBase
{
    private readonly IMediator _mediator;

    public DocumentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize, HttpPost]
    public async Task<IActionResult> Upload( DocumentType documentType, IFormFile file)
    {
        return Ok(await _mediator.Send(new UploadDocumentRequest(documentType, file,
            Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    [Authorize, HttpPut]
    public async Task<IActionResult> Edit( DocumentType documentType, IFormFile file)
    {
        return Ok(await _mediator.Send(new EditDocumentCommand(documentType, file,
            Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    [Authorize, HttpDelete]
    public async Task<IActionResult> Delete(DocumentType documentType)
    {
        return Ok(await _mediator.Send(new DeleteDocumentCommand(documentType,
            Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }
}