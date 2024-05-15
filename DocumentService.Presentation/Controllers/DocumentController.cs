using DocumentService.Application.Features.Commands.Documents.DeleteDocument;
using DocumentService.Application.Features.Commands.Documents.EditDocument;
using DocumentService.Application.Features.Commands.Documents.UploadDocument;
using DocumentService.Application.Features.Queries.DownloadFile;
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

    [Authorize, HttpGet, Route("download")]
    public async Task<IActionResult> Download(DocumentType documentType)
    {
        var result = await _mediator.Send(new DownloadFileCommand(documentType,
            Guid.Parse(User.FindFirst("UserId")!.Value!)));
        
        return File(result.Item1, result.Item2, result.Item3);
    }
    
    [Authorize, HttpPost, Route("upload")]
    public async Task<IActionResult> Upload( DocumentType documentType, IFormFile file)
    {
        return Ok(await _mediator.Send(new UploadDocumentRequest(documentType, file,
            Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    [Authorize, HttpPut,Route("edit")]
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