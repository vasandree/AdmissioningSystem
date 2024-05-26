using AdminPanel.Application.Features.Commands.Users.GetAllUsers;
using AdminPanel.Application.Features.Queries.Managers.GetAllManagers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.MVC.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Users(int page = 1, int pageSize = 10)
    {
        var command = new GetAllUsersCommand(page, pageSize);
        var result = await _mediator.Send(command);
        
        return View(result);
    }
}