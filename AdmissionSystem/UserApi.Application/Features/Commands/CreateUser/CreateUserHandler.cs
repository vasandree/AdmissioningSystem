using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using UserApi.Application.Contracts.Persistence;
using UserApi.Application.Dtos.Responses;
using UserApi.Domain.DbEntities;

namespace UserApi.Application.Features.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUser, TokenResponseDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;
    private readonly IConfiguration _configuration;

    public CreateUserHandler(IUserRepository repository, IMapper mapper, IJwtService jwt, IConfiguration configuration)
    {
        _repository = repository;
        _mapper = mapper;
        _jwt = jwt;
        _configuration = configuration;
    }

    public async Task<TokenResponseDto> Handle(CreateUser request, CancellationToken cancellationToken)
    {
        var user = _mapper.Map<ApplicationUser>(request.NewUser);
        var refreshToken = _jwt.GenerateRefreshTokenString();
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryDate = DateTime.UtcNow.AddDays(_configuration.GetValue<int>("Jwt:RefreshDaysLifeTime"));
        await _repository.CreateUser(user, request.NewUser.Password);
        return new TokenResponseDto
        {
            AcccessToken = _jwt.GenerateTokenString(request.NewUser.Email),
            RefreshToken = refreshToken
        };
    }
}