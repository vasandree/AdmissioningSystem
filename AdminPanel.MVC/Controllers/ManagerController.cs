using AdminPanel.Application.Features.Commands.Users.GetAllApplicants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdminPanel.MVC.Controllers;

public class ManagerController : Controller
{
    private readonly IMediator _mediator;

    public ManagerController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Applicants(int pageNumber = 1, int pageSize = 10)
    {
        var command = new GetAllApplicantsCommand(pageNumber, pageSize);
        var result = await _mediator.Send(command);

        return View(result);
    }
}