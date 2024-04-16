using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.Dtos.Requests;
using UserApi.Application.Features.Commands.EditPassword;
using UserApi.Application.Features.Commands.ForgetPassword;
using UserApi.Application.Features.Commands.UpdateUserProfile;
using UserApi.Application.Features.Queries.GetByEmail;

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
        return Ok(await _mediator.Send(new GetByEmailQuery(User.Identity.Name)));
    }
    
    [HttpPut, Authorize]
    [Route("edit")]
    public async Task<IActionResult> EditProfile([FromBody] EditProfileDto editProfileDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new UpdateUserProfile(User.Identity.Name, editProfileDto)));
    }

    [HttpPut, Authorize]
    [Route("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] PasswordChangeDto passwordChangeDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new EditPasswordCommand(User.Identity.Name, passwordChangeDto)));
    }

    [HttpPost, Authorize]
    [Route("forget-password-email")]
    public async Task<IActionResult> SendForgetPassword()
    {
        return Ok();
    }
    
    [HttpPut, Authorize]
    [Route("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto forgetPasswordDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new ForgetPasswordCommand(User.Identity.Name, forgetPasswordDto)));
    }
}