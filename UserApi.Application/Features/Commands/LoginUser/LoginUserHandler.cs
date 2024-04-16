using MediatR;
using Microsoft.Extensions.Configuration;
using UserApi.Application.Contracts.Persistence;
using UserApi.Application.Dtos.Responses;

namespace UserApi.Application.Features.Commands.LoginUser;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, TokenResponseDto>
{
    private readonly IUserRepository _repository;
    private readonly IJwtService _jwt;
    private readonly IConfiguration _configuration;

    public LoginUserHandler(IUserRepository repository, IJwtService jwt, IConfiguration configuration)
    {
        _repository = repository;
        _jwt = jwt;
        _configuration = configuration;
    }


    public async Task<TokenResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmail(request.LoginUserDto.Email);
        if (user == null) throw new Exception("No such user");
        if (!await _repository.CheckPassword(user, request.LoginUserDto.Password))
            throw new Exception("Wrong password");
        var refreshToken = _jwt.GenerateRefreshTokenString();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshDaysLifeTime"));
        await _repository.UpdateAsync(user);
        return new TokenResponseDto()
        {
            AcccessToken = _jwt.GenerateTokenString(request.LoginUserDto.Email),
            RefreshToken = refreshToken
        };
    }
}