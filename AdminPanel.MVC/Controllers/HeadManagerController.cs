using AdminPanel.Application.Features.Queries.Managers.GetAllManagers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.MVC.Controllers;

[Authorize(Roles = "Admin, HeadManager")]
public class HeadManagerController : Controller
{
    private readonly IMediator _mediator;

    public HeadManagerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Managers()
    {
        var managers = await _mediator.Send(new GetAllManagersCommand());
        return View(managers);
    }
}