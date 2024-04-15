using MediatR;
using UserApi.Application.Contracts.Persistence;

namespace UserApi.Application.Features.Commands.EditPassword;

public class EditPasswordHandler : IRequestHandler<EditPasswordCommand>
{
    private readonly IUserRepository _repository;

    public EditPasswordHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(EditPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmail(request.Email);
        await _repository.ChangePassword(user, request.Dto.OldaPassword, request.Dto.NewPassword);
    }
}