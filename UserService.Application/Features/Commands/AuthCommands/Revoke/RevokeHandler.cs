using Common.Models.Exceptions;
using MediatR;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.Features.Commands.AuthCommands.Revoke;

public class RevokeHandler : IRequestHandler<RevokeCommand, Unit>
{
    private readonly ITokenRepository _repository;
    private readonly IJwtService _jwt;

    public RevokeHandler(ITokenRepository repository, IJwtService jwt)
    {
        _repository = repository;
        _jwt = jwt;
    }


    public async Task<Unit> Handle(RevokeCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwt.GetTokenPrincipal(request.RevokeTokenDto.AccessToken);
        
        if (principal == null)
            throw new Unauthorized();
        
        if(Guid.Parse(principal.FindFirst("UserId")!.Value) != request.UserId)
            throw new Forbidden("Access token does not belong to the specified user.");
        
        if (!_repository.CheckIfExists(request.RevokeTokenDto.RefreshToken) || await _repository.CheckIfExpired(request.RevokeTokenDto.RefreshToken) )
            throw new NotFound("Provided refresh token does not exist");
        
        if (!await _repository.CheckIfItBelongsToUser(request.RevokeTokenDto.RefreshToken, request.UserId))
            throw new Forbidden("Provided token does not belong to this user");

        var token = await _repository.GetToken(request.RevokeTokenDto.RefreshToken);

        //todo: expire access token
        
        await _repository.DeleteAsync(token);

        return Unit.Value;
    }
}