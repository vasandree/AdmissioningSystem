using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Dtos.Requests;
using UserService.Application.Features.Commands.ProfileCommands.EditPassword;
using UserService.Application.Features.Commands.ProfileCommands.ForgetPassword;
using UserService.Application.Features.Commands.ProfileCommands.SendEmailCode;
using UserService.Application.Features.Commands.ProfileCommands.UpdateUserProfile;
using UserService.Application.Features.Queries.GetByEmail;

namespace UserApi.Controllers;

[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetProfile()
    {
        return Ok(await _mediator.Send(new GetUserByIdQuery(Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    [HttpPut, Authorize]
    [Route("edit")]
    public async Task<IActionResult> EditProfile([FromBody] EditProfileDto editProfileDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new UpdateUserProfile(Guid.Parse(User.FindFirst("UserId")!.Value!),
            editProfileDto)));
    }

    [HttpPut, Authorize]
    [Route("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto passwordChangeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new EditPasswordCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            passwordChangeDto)));
    }

    [HttpPost, Authorize]
    [Route("forget-password-email")]
    public async Task<IActionResult> SendForgetPassword()
    {
        return Ok(await _mediator.Send(new SendEmailCode(Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    [HttpPut, Authorize]
    [Route("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new ForgetPasswordCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            forgetPasswordDto)));
    }
}