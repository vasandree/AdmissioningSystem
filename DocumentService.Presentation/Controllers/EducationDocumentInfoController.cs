using System.ComponentModel.DataAnnotations;
using DocumentService.Application.Dtos.Requests;
using DocumentService.Application.Features.Commands.EducationDocumentInfo.AddEducationDocumentInfo;
using DocumentService.Application.Features.Commands.EducationDocumentInfo.DeleteEducationDocumentInfo;
using DocumentService.Application.Features.Commands.EducationDocumentInfo.EditEducationDocumentInfo;
using DocumentService.Application.Features.Queries.GetEducationDocumentInfo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Presentation.Controllers;

[ApiController, Route("api/education_document_info")]
public class EducationDocumentInfoController : ControllerBase
{
    private readonly IMediator _mediator;

    public EducationDocumentInfoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetEducationDocumentInfo()
    {
        return Ok(await _mediator.Send(
            new GetEducationDocumentInfoCommand(Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    [HttpPost, Authorize]
    public async Task<IActionResult> AddEducationDocumentInfo(
        [FromBody] EducationDocumentRequest educationDocumentRequest)
    {
        return Ok(await _mediator.Send(new AddEducationDocumentInfoCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            educationDocumentRequest)));
    }

    [HttpPut, Authorize]
    public async Task<IActionResult> EditEducationDocumentInfo(
        [FromBody] EducationDocumentRequest educationDocumentRequest)
    {
        return Ok(await _mediator.Send(
            new EditEducationDocumentInfoCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
                educationDocumentRequest)));
    }

    [HttpDelete, Authorize]
    public async Task<IActionResult> DeleteEducationDocumentInfo()
    {
        return Ok(await _mediator.Send(
            new DeleteEducationDocumentInfoCommand(Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }
}