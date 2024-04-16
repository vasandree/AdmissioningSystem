using MediatR;
using UserApi.Application.Contracts.Persistence;

namespace UserApi.Application.Features.Commands.ForgetPassword;

public class ForgetPasswordHandler : IRequestHandler<ForgetPasswordCommand, Unit>
{
    private readonly IUserRepository _repository;

    public ForgetPasswordHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(ForgetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmail(request.Email);
        if (user == null) throw new Exception("No such user");
        if (user.ConfirmCode != request.ForgetPassword.ConfirmCode) throw new Exception("Confirm code is incorrect");
        await _repository.ResetPassword(user, request.ForgetPassword.NewPassword);
        return Unit.Value;
    }
}