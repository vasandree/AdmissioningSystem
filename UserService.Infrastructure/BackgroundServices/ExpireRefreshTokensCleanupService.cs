using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace UserService.Infrastructure.BackgroundServices;

public class ExpireRefreshTokensCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public ExpireRefreshTokensCleanupService(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();
                await CleanExpiredRefreshTokens(dbContext);
            }
            
            await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
        }
    }

    private async Task CleanExpiredRefreshTokens(UserDbContext dbContext)
    {
        var currentTime = DateTime.UtcNow;
        var expiredTokens = await dbContext.RefreshTokens
            .Where(rt => rt.ExpiryDate < currentTime)
            .ToListAsync();

        dbContext.RefreshTokens.RemoveRange(expiredTokens);
        await dbContext.SaveChangesAsync();
    }
}