using AutoMapper;
using Common.Models.Exceptions;
using Common.Services.Repository;
using MediatR;
using Microsoft.Extensions.Configuration;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;
using UserService.Application.Dtos.Responses;

namespace UserService.Application.Features.Commands.AuthCommands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, TokenResponseDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;
    private readonly IConfiguration _configuration;
    private readonly ITokenRepository _token;

    public CreateUserHandler(IUserRepository repository, IMapper mapper, IJwtService jwt, IConfiguration configuration, ITokenRepository token)
    {
        _repository = repository;
        _mapper = mapper;
        _jwt = jwt;
        _configuration = configuration;
        _token = token;
    }

    public async Task<TokenResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (await _repository.GetByEmail(request.NewUser.Email) != null) 
            throw new Conflict("User with this email already exists");
        
        var user = _mapper.Map<ApplicationUser>(request.NewUser);
        var refreshToken = _jwt.GenerateRefreshTokenString();

        var refreshTokenEntity = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            ExpiryDate = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshDaysLifeTime")),
            User = user
        };
        await _repository.CreateUser(user, request.NewUser.Password);
        await _token.CreateAsync(refreshTokenEntity);
        
        return new TokenResponseDto
        {
            AcccessToken = _jwt.GenerateTokenString(user.Email!, user.Id),
            RefreshToken = refreshToken
        };
    }
}