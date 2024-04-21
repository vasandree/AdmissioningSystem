using Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using UserApi.Application.Contracts.Persistence;
using UserApi.Application.Dtos.Responses;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Commands.GetNewTokens;

public class GetNewTokensHandler : IRequestHandler<GetNewTokensCommand, TokenResponseDto>
{
    private readonly IUserRepository _user;
    private readonly IJwtService _jwt;
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<RefreshToken> _generic;

    public GetNewTokensHandler(IUserRepository repository, IJwtService jwt, IConfiguration configuration, IGenericRepository<RefreshToken> generic)
    {
        _user = repository;
        _jwt = jwt;
        _configuration = configuration;
        _generic = generic;
    }


    public async Task<TokenResponseDto> Handle(GetNewTokensCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwt.GetTokenPrincipal(request.RefreshTokenDto.AccessToken);
        if (principal?.Identity?.Name is null) 
            throw new Unauthorized();;

        var token = (await _generic.Find(x => x.Token == request.RefreshTokenDto.RefreshToken))[0];
        
        var user = await _user.GetByEmail(principal.Identity.Name);
        if (user is null || token.Token != request.RefreshTokenDto.RefreshToken ||
            token.ExpiryDate < DateTime.UtcNow) 
            throw new Unauthorized();
        
        var newRefreshToken = _jwt.GenerateRefreshTokenString();
        token.Token = newRefreshToken;
        token.ExpiryDate = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshDaysLifeTime"));
        await _generic.UpdateAsync(token);
        
        return new TokenResponseDto
        {
            AcccessToken = _jwt.GenerateTokenString(user.Email!),
            RefreshToken = newRefreshToken
        };
    }
}