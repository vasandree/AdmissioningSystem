using Common.Exceptions;
using MediatR;
using UserApi.Application.Contracts.Persistence;

namespace UserApi.Application.Features.Commands.EditPassword;

public class EditPasswordHandler : IRequestHandler<EditPasswordCommand, Unit>
{
    private readonly IUserRepository _repository;

    public EditPasswordHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(EditPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmail(request.Email);
        if (user == null) throw new BadRequest("No such user");
        await _repository.ChangePassword(user, request.Dto.OldaPassword, request.Dto.NewPassword);
        return Unit.Value;
    }
}