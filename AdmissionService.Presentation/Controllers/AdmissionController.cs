using System.ComponentModel.DataAnnotations;
using AdmissionService.Application.Dtos.Requests;
using AdmissionService.Application.Features.Commands.CreateNewAdmission;
using AdmissionService.Application.Features.Commands.DeleteAdmission;
using AdmissionService.Application.Features.Commands.EditPriority;
using AdmissionService.Application.Features.Queries.GetAllMyAdmissions;
using AdmissionService.Application.Features.Queries.GetMyAdmissionById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdmissionService.Presentation.Controllers;

[ApiController, Route("api/applicant")]
public class AdmissionController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdmissionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, Authorize, Route("admissions")]
    public async Task<IActionResult> GetMyAdmissions()
    {
        return Ok(await _mediator.Send(new GetAllMyAdmissionsCommand(Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    [HttpGet, Authorize, Route("admission/{admissionId}")]
    public async Task<IActionResult> GetMyAdmission([Required]Guid admissionId)
    {
        return Ok(await _mediator.Send(new GetAdmissionByIdCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            admissionId)));
    }

    [HttpPost, Authorize, Route("admission")]
    public async Task<IActionResult> CreateNewAdmission([FromBody] CreateAdmissionRequest createAdmissionRequest)
    {
        return Ok(await _mediator.Send(new CreateNewAdmissionCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            createAdmissionRequest)));
    }

    [HttpPut, Authorize, Route("admission/{admissionId}/priority")]
    public async Task<IActionResult> ChangeAdmissionPriority([Required] Guid admissionId, [Required]int priority)
    {
        return Ok(await _mediator.Send(new EditPriorityCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            admissionId, priority)));
    }

    [HttpDelete, Authorize, Route("admission")]
    public async Task<IActionResult> DeleteAdmission([Required] Guid admissionId)
    {
        return Ok(await _mediator.Send(new DeleteAdmissionCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            admissionId)));
    }
}