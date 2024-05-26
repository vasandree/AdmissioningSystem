using System.Security.Claims;
using AdminPanel.Application.Features.Commands.Account.Login;
using AdminPanel.Application.Features.Commands.Managers.EditManagerInfo;
using AdminPanel.Application.Features.Queries.GetManagerProfile;
using AdminPanel.MVC.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Dtos.Requests;

namespace AdminPanel.MVC.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }


    [HttpGet]
    [Authorize]
    public IActionResult PasswordChange()
    {
        return View();
    }


    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        try
        {
            var loginCommand = new LoginCommand(new LoginUserDto
            {
                Email = model.Email,
                Password = model.Password,
            });

            await _mediator.Send(loginCommand);

            return RedirectToAction("Index", "Home");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
    }

    [HttpPost]
    public IActionResult Logout()
    {
        return Redirect("/");
    }

    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            var info = await _mediator.Send(new GetManagerProfileCommand(Guid.Parse(userId!)));

            var model = new ProfileViewModel
            {
                Id = info.Id,
                FullName = info.FullName,
                Email = info.Email,
                Faculty = info.Faculty,
            };
            return View(model);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> ChangeEmailAndFullname(ProfileViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View("Profile", model);
        }

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        try
        {
            await _mediator.Send(new EditManagerInfoCommand(Guid.Parse(userId!), Guid.Parse(userId!), model.FullName,
                model.Email));
            return Redirect("Profile");
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View("Profile", model);
        }
        
    }


    [HttpPost]
    public Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
    {
        throw new NotImplementedException();
    }
}