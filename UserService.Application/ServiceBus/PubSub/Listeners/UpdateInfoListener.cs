using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.ServiceBus.PubSub.Listeners;

public class UpdateInfoListener : BackgroundService
{
    private IBus _bus;
    private IServiceProvider _serviceProvider;

    public UpdateInfoListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<UpdateEmailAndFullNameMessage>("update_info_subscription_id",
            UpdateUserInfo);
    }

    private async Task UpdateUserInfo(UpdateEmailAndFullNameMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var user = await repository.GetById(message.UserId);

            user.FullName = message.FullName;
            user.Email = message.Email;
            await repository.UpdateAsync(user);
        }
    }
}