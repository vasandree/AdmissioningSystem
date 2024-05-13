using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Dtos.Requests;
using UserService.Application.Features.Commands.AuthCommands.CreateUser;
using UserService.Application.Features.Commands.AuthCommands.GetNewTokens;
using UserService.Application.Features.Commands.AuthCommands.LoginUser;
using UserService.Application.Features.Commands.AuthCommands.Revoke;
using UserService.Application.Features.Commands.AuthCommands.RevokeAllTokens;

namespace UserApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto registerUserDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new CreateUserCommand(registerUserDto)));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new LoginUserCommand(loginUserDto)));
    }

    [HttpPost, Authorize]
    [Route("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new GetNewTokensCommand(refreshTokenDto,
            Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }

    [HttpDelete, Authorize]
    [Route("revoke")]
    public async Task<IActionResult> Revoke([FromBody] RevokeTokenDto revokeTokenDto)
    {
        return Ok(await _mediator.Send(new RevokeCommand(Guid.Parse(User.FindFirst("UserId")!.Value!),
            revokeTokenDto)));
    }

    [HttpDelete, Authorize]
    [Route("revoke_all_tokens")]
    public async Task<IActionResult> RevokeAllTokens()
    {
        return Ok(await _mediator.Send(new RevokeAllTokensCommand(Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }
}