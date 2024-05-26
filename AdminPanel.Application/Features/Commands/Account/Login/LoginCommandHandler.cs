using AdminPanel.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace AdminPanel.Application.Features.Commands.Account.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Unit>
{
    private readonly UserManager<BaseManager> _manager;
    private readonly SignInManager<BaseManager> _signInManager;

    public LoginCommandHandler(UserManager<BaseManager> manager, SignInManager<BaseManager> signInManager)
    {
        _manager = manager;
        _signInManager = signInManager;
    }

    public async Task<Unit> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var result = await _signInManager.PasswordSignInAsync(request.LoginUserDto.Email, request.LoginUserDto.Password,
            true, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return Unit.Value;
        }

        if (result.IsLockedOut)
        {
            throw new InvalidOperationException("User account is locked out.");
        }


        throw new InvalidOperationException("Invalid login attempt.");
    }
}