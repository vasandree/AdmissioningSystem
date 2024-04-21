using AutoMapper;
using MediatR;
using Microsoft.Extensions.Configuration;
using UserApi.Application.Contracts.Persistence;
using UserApi.Application.Dtos.Responses;
using UserApi.Domain.DbEntities;
using Conflict = Common.Exceptions.Conflict;

namespace UserApi.Application.Features.Commands.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, TokenResponseDto>
{
    private readonly IUserRepository _repository;
    private readonly IMapper _mapper;
    private readonly IJwtService _jwt;
    private readonly IConfiguration _configuration;
    private readonly IGenericRepository<RefreshToken> _generic;

    public CreateUserHandler(IUserRepository repository, IMapper mapper, IJwtService jwt, IConfiguration configuration, IGenericRepository<RefreshToken> generic)
    {
        _repository = repository;
        _mapper = mapper;
        _jwt = jwt;
        _configuration = configuration;
        _generic = generic;
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
        await _generic.CreateAsync(refreshTokenEntity);
        
        return new TokenResponseDto
        {
            AcccessToken = _jwt.GenerateTokenString(request.NewUser.Email),
            RefreshToken = refreshToken
        };
    }
}