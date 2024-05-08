using System.Security.Claims;

namespace UserApi.Application.Contracts.Persistence;

public interface IJwtService
{
    string GenerateTokenString(string email, Guid id);
    ClaimsPrincipal? GetTokenPrincipal(string token);
    string GenerateRefreshTokenString();
    
}