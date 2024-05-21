using MediatR;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.Features.Commands.AuthCommands.RevokeAllTokens;

public class RevokeAllTokensCommandHandler : IRequestHandler<RevokeAllTokensCommand, Unit>
{
    private readonly IGenericRepository<RefreshToken> _repository;

    public RevokeAllTokensCommandHandler(IGenericRepository<RefreshToken> repository)
    {
        _repository = repository;
    }

    public async Task<Unit> Handle(RevokeAllTokensCommand request, CancellationToken cancellationToken)
    {
        var tokens = await _repository.Find(x => x.UserId == request.UserId);

        foreach (var refreshToken in tokens) await _repository.DeleteAsync(refreshToken);

        return Unit.Value;
    }
}