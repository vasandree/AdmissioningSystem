using Common.Models.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Commands.AuthCommands.GetNewTokens;

public class GetNewTokensHandler : IRequestHandler<GetNewTokensCommand, TokenResponseDto>
{
    private readonly IJwtService _jwt;
    private readonly IConfiguration _configuration;
    private readonly ITokenRepository _repository;
    private readonly IUserRepository _user;

    public GetNewTokensHandler( IJwtService jwt, IConfiguration configuration,
        ITokenRepository repository, IUserRepository user)
    {
        _jwt = jwt;
        _configuration = configuration;
        _repository = repository;
        _user = user;
    }


    public async Task<TokenResponseDto> Handle(GetNewTokensCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwt.GetTokenPrincipal(request.RefreshTokenDto.AccessToken);

        if (principal == null)
            throw new Unauthorized();

        var userIdFromToken = Guid.Parse(principal.FindFirst("UserId")!.Value);
        
        if (!_repository.CheckIfExists(request.RefreshTokenDto.RefreshToken) ||
            await _repository.CheckIfExpired(request.RefreshTokenDto.RefreshToken))
            throw new NotFound("Provided refresh token does not exist or has expired.");

        if (!await _repository.CheckIfItBelongsToUser(request.RefreshTokenDto.RefreshToken, userIdFromToken))
            throw new Forbidden("Provided token does not belong to this user.");

        var token = await _repository.GetToken(request.RefreshTokenDto.RefreshToken);
        
        await _repository.DeleteAsync(token);

        var newRefreshToken = _jwt.GenerateRefreshTokenString();
    
        var user = await _user.GetById(userIdFromToken);
        await _repository.CreateAsync(new RefreshToken
        {
            UserId = user!.Id,
            Token = newRefreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshDaysLifeTime")),
            User = user
        });

        return new TokenResponseDto
        {
            AcccessToken = _jwt.GenerateTokenString(user.Email!, user.Id),
            RefreshToken = newRefreshToken
        };
    }
}