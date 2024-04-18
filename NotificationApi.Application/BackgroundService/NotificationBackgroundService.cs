using Microsoft.Extensions.DependencyInjection;
using NotificationApi.Application.Сonsumers;

namespace NotificationApi.Application.BackgroundService;

public class NotificationBackgroundService : Microsoft.Extensions.Hosting.BackgroundService
{
    private readonly IServiceProvider _serviceProvider;

    public NotificationBackgroundService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {

        using (var scope = _serviceProvider.CreateScope())
        {
            var consumer = scope.ServiceProvider.GetRequiredService<IForgetPasswordConsumer>();
            await consumer.StartConsuming(cancellationToken);
        }
    }
}