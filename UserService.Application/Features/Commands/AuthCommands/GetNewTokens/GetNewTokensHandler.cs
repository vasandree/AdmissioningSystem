using Common.Exceptions;
using MediatR;
using Microsoft.Extensions.Configuration;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Commands.AuthCommands.GetNewTokens;

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
        
        if (principal == null)
            throw new Unauthorized();
        
       
        var token = (await _generic.Find(x => x.Token == request.RefreshTokenDto.RefreshToken))[0];
        if (token == null) throw new BadRequest("Provided refresh token does not exist");
        
        var user = await _user.GetById(Guid.Parse(principal.FindFirst("UserId").Value));
        if (token.UserId != user.Id) throw new BadRequest("Provided refresh token doesn't belong to this user");
        
        if (user is null || token.Token != request.RefreshTokenDto.RefreshToken ||
            token.ExpiryDate < DateTime.UtcNow) 
            throw new Unauthorized();

        await _generic.DeleteAsync(token);

        var newRefreshToken = _jwt.GenerateRefreshTokenString();
        await _generic.CreateAsync(new RefreshToken
        {
            UserId = user.Id,
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