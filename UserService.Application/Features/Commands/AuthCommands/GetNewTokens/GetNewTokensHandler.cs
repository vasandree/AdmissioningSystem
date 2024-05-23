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

    public GetNewTokensHandler(IUserRepository repository, IJwtService jwt, IConfiguration configuration,
        ITokenRepository repository1, IUserRepository user)
    {
        _jwt = jwt;
        _configuration = configuration;
        _repository = repository1;
        _user = user;
    }


    public async Task<TokenResponseDto> Handle(GetNewTokensCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwt.GetTokenPrincipal(request.RefreshTokenDto.AccessToken);

        if (principal == null)
            throw new Unauthorized();


        if (Guid.Parse(principal.FindFirst("UserId")!.Value) != request.UserId)
            throw new Forbidden("Access token does not belong to the specified user.");

        if (!_repository.CheckIfExists(request.RefreshTokenDto.RefreshToken) ||
            await _repository.CheckIfExpired(request.RefreshTokenDto.RefreshToken))
            throw new NotFound("Provided refresh token does not exist");

        if (!await _repository.CheckIfItBelongsToUser(request.RefreshTokenDto.RefreshToken, request.UserId))
            throw new Forbidden("Provided token does not belong to this user");

        var token = await _repository.GetToken(request.RefreshTokenDto.RefreshToken);

        //todo: expire access token

        await _repository.DeleteAsync(token);

        var newRefreshToken = _jwt.GenerateRefreshTokenString();
        
        var user = await _user.GetById(request.UserId);
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