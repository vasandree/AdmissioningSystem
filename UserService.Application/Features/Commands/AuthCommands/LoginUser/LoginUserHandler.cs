using Common.Exceptions;
using Common.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Commands.AuthCommands.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, TokenResponseDto>
{
    private readonly IUserRepository _user;
    private readonly IJwtService _jwt;
    private readonly IConfiguration _configuration;
    private readonly ITokenRepository _token;

    public LoginUserHandler(IUserRepository repository, IJwtService jwt, IConfiguration configuration, ITokenRepository token)
    {
        _user = repository;
        _jwt = jwt;
        _configuration = configuration;
        _token = token;
    }


    public async Task<TokenResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _user.GetByEmail(request.LoginUserDto.Email);
        if (user == null) throw new BadRequest("There is no user with such email");
        
        if (!await _user.CheckPassword(user, request.LoginUserDto.Password))
            throw new BadRequest("Wrong password");
        
        var refreshToken = _jwt.GenerateRefreshTokenString();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshDaysLifeTime")),
            User = user
        };

        await _token.CreateAsync(refreshTokenEntity);
        
        return new TokenResponseDto()
        {
            AcccessToken = _jwt.GenerateTokenString(user.Email!, user.Id),
            RefreshToken = refreshToken
        };
    }
}