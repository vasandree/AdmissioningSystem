using AdmissionService.Application.Dtos.Requests;
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

    [HttpGet, Authorize, Route("admission")]
    public async Task<IActionResult> GetMyAdmission([FromBody] AdmissionRequestDto admissionRequestDto)
    {
        return Ok(await _mediator.Send(new GetAdmissionByIdCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            admissionRequestDto)));
    }

    [HttpPut, Authorize, Route("admission/priority")]
    public async Task<IActionResult> ChangeAdmissionPriority(
        [FromBody] ChangeAdmissionPriorityDto changeAdmissionPriority)
    {
        return Ok(await _mediator.Send(new EditPriorityCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            changeAdmissionPriority)));
    }

    [HttpDelete, Authorize, Route("admission")]
    public async Task<IActionResult> DeleteAdmission([FromBody] AdmissionRequestDto admissionRequestDto)
    {
        return Ok(await _mediator.Send(new DeleteAdmissionCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            admissionRequestDto)));
    }
}