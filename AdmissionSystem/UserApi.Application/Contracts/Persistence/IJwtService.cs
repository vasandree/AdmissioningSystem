using System.Security.Claims;

namespace UserApi.Application.Contracts.Persistence;

public interface IJwtService
{
    string GenerateTokenString(string email);
    ClaimsPrincipal? GetTokenPrincipal(string token);
    string GenerateRefreshTokenString();
    
}