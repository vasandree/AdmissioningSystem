using MediatR;
using UserApi.Application.Contracts.Persistence;

namespace UserApi.Application.Features.Commands.Revoke;

public class RevokeHandler : IRequestHandler<RevokeCommand, Unit>
{
    private readonly IUserRepository _repository;

    public RevokeHandler(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmail(request.Email);
        if (user == null) 
        {
            throw new Exception("No such user");
        }
        user.RefreshToken = null;
        await _repository.UpdateAsync(user);
        
        // Return Unit to indicate success (assuming RevokeCommand doesn't return any specific result)
        return Unit.Value;
    }
}