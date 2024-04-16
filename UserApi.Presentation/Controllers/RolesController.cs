using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserApi.Application.Features.Queries.GetUserRoles;

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
        return Ok(await _mediator.Send(new GetUserRolesQuery(User.Identity.Name)));
    }
}