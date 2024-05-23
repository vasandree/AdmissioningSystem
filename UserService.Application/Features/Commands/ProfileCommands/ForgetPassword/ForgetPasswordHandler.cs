using Common.Models.Exceptions;
using MediatR;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.Features.Commands.ProfileCommands.ForgetPassword;

public class ForgetPasswordHandler : IRequestHandler<ForgetPasswordCommand, Unit>
{
    private readonly IUserRepository _repository;

    public ForgetPasswordHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById(request.Id);
        if (user == null) throw new BadRequest("No such user");
        if (user.ConfirmCode != request.ForgetPassword.ConfirmCode) 
            throw new BadRequest("Confirm code is incorrect");
        await _repository.ResetPassword(user, request.ForgetPassword.NewPassword);
        return Unit.Value;
    }
}