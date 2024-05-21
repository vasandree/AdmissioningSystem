using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Features.Queries.RolesCommands.GetUserRoles;

namespace UserApi.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet, Authorize]
    public async Task<IActionResult> GetRoles()
    {
        return Ok(await _mediator.Send(new GetUserRolesQuery(Guid.Parse(User.FindFirst("UserId")!.Value!))));
    }
}