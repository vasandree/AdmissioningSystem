using Common.Exceptions;
using MediatR;
using UserApi.Application.Contracts.Persistence;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Commands.Revoke;

public class RevokeHandler : IRequestHandler<RevokeCommand, Unit>
{
    private readonly IGenericRepository<RefreshToken> _repository;

    public RevokeHandler(IGenericRepository<RefreshToken> repository)
    {
        _repository = repository;
    }


    public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
        var token =
            await _repository.Find(x => x.Token == request.RefreshToken);
        
        await _repository.DeleteAsync(token[0]);
        
        return Unit.Value;
    }
}