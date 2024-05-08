using DocumentService.Application.Dtos;
using DocumentService.Application.Features.Commands.UploadEducationDocument;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Presentation.Controllers;

[ApiController, Route("api/documents/education_document")]
public class EducationDocumentController : ControllerBase
{
    private readonly IMediator _mediator;

    public EducationDocumentController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> Upload([FromBody] UploadEducationDocumentRequest request )
    {
        return Ok(await _mediator.Send(new UploadEducationDocumentCommand(request.File,
            Guid.Parse(User.FindFirst("UserId")!.Value!), request.DocumentTypeId, request.Name)));
    }
}