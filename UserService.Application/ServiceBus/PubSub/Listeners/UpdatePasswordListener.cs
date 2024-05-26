using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.ServiceBus.PubSub.Listeners;

public class UpdatePasswordListener: BackgroundService
{
    private IBus _bus;
    private IServiceProvider _serviceProvider;

    public UpdatePasswordListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<EditPasswordMessage>("update_info_subscription_id",
            UpdateUserInfo);
    }

    private async Task UpdateUserInfo(EditPasswordMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var user = await repository.GetById(message.UserId);

            await repository.ChangePassword(user!, message.OldPassword, message.NewPassword);
        }
    }
}