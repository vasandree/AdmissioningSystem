using MediatR;
using Microsoft.Extensions.Configuration;
using UserApi.Application.Contracts.Persistence;
using UserApi.Application.Dtos.Responses;

namespace UserApi.Application.Features.Commands.GetNewTokens;

public class GetNewTokensHandler : IRequestHandler<GetNewTokensCommand, TokenResponseDto>
{
    private readonly IUserRepository _repository;
    private readonly IJwtService _jwt;
    private readonly IConfiguration _configuration;

    public GetNewTokensHandler(IUserRepository repository, IJwtService jwt, IConfiguration configuration)
    {
        _repository = repository;
        _jwt = jwt;
        _configuration = configuration;
    }


    public async Task<TokenResponseDto> Handle(GetNewTokensCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetByEmail(request.Email);
        if (user.RefreshTokenExpiryDate < DateTime.UtcNow) throw new Exception("Unauthorized");
        var newRefreshToken = _jwt.GenerateRefreshTokenString();
        user.RefreshToken = newRefreshToken;
        user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshDaysLifeTime"));
        await _repository.UpdateAsync(user);
        return new TokenResponseDto
        {
            AcccessToken = _jwt.GenerateTokenString(request.Email),
            RefreshToken = newRefreshToken
        };
    }
}