using Common.Repository;
using Microsoft.EntityFrameworkCore;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;
using UserService.Infrastructure;

namespace UserService.Persistence.Repositories;

public class TokenRepository : GenericRepository<RefreshToken>, ITokenRepository
{
    private readonly UserDbContext _context;
    
    public TokenRepository(UserDbContext context) : base(context)
    {
        _context = context;
    }

    public bool CheckIfExists(string token)
    {
        return _context.RefreshTokens.Any(x => x.Token == token);
    }

    public async Task<RefreshToken> GetToken(string token)
    {
        return (await _context.RefreshTokens.FirstOrDefaultAsync(x=>x.Token == token))!;
    }

    public Task<bool> CheckIfItBelongsToUser(string token, Guid userId)
    {
        return  Task.FromResult(GetToken(token).Result.UserId == userId);
    }

    public Task<bool> CheckIfExpired(string token)
    {
        return  Task.FromResult(GetToken(token).Result.ExpiryDate < DateTime.Now);
    }
}