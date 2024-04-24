using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.Dtos.Requests;
using UserApi.Application.Features.Commands.CreateUser;
using UserApi.Application.Features.Commands.GetNewTokens;
using UserApi.Application.Features.Commands.LoginUser;
using UserApi.Application.Features.Commands.Revoke;

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
        return Ok( await _mediator.Send(new CreateUserCommand(registerUserDto)));
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginUserDto loginUserDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new LoginUserCommand(loginUserDto)));
    }

    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        return Ok(await _mediator.Send(new GetNewTokensCommand(refreshTokenDto)));
    }

    [HttpPost, Authorize]
    [Route("revoke")]
    public async Task<IActionResult> Revoke()
    {
        return Ok(await _mediator.Send(new RevokeCommand(User.Identity.Name)));
    }
}