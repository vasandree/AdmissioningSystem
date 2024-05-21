using Common.Exceptions;
using MediatR;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.Features.Commands.AuthCommands.Revoke;

public class RevokeHandler : IRequestHandler<RevokeCommand, Unit>
{
    private readonly IGenericRepository<RefreshToken> _repository;

    public RevokeHandler(IGenericRepository<RefreshToken> repository)
    {
        _repository = repository;
    }


    public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
        var tokens = await _repository.Find(x => x.Token == request.RevokeTokenDto.RefreshToken);
        if (tokens[0] == null) throw new BadRequest("Provided refresh token does not exist");

        var token = tokens[0];
        if (token.UserId != request.UserId) throw new BadRequest("Provided token does not belong to logged user");

        await _repository.DeleteAsync(token);
        
        return Unit.Value;
    }
}