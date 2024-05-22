using Common.Repository;
using UserApi.Domain.DbEntities;

namespace UserService.Application.Contracts.Persistence;

public interface ITokenRepository : IGenericRepository<RefreshToken>
{
    bool CheckIfExists(string token);
    
    Task<RefreshToken> GetToken(string token);

    Task<bool> CheckIfItBelongsToUser(string token, Guid userId);

    Task<bool> CheckIfExpired(string token);
}