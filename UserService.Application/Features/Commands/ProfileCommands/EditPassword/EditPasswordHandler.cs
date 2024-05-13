using Common.Exceptions;
using MediatR;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.Features.Commands.ProfileCommands.EditPassword;

public class EditPasswordHandler : IRequestHandler<EditPasswordCommand, Unit>
{
    private readonly IUserRepository _repository;

    public EditPasswordHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(EditPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetById(request.UserId);
        if (user == null) throw new BadRequest("No such user");
        await _repository.ChangePassword(user, request.Dto.OldaPassword, request.Dto.NewPassword);
        return Unit.Value;
    }
}