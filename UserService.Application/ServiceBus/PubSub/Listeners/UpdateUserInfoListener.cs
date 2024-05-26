using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.ServiceBus.PubSub.Listeners;

public class UpdateUserInfoListener : BackgroundService
{
    private IBus _bus;
    private IServiceProvider _serviceProvider;

    public UpdateUserInfoListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<UpdateUserInfoMessage>("update_info_subscription_id",
            UpdateUserInfo);
    }

    private async Task UpdateUserInfo(UpdateUserInfoMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();

            var user = await repository.GetById(message.UserId);

            user.FullName = message.FullName;
            user.Gender = !string.IsNullOrEmpty(message.Gender.ToString()) ? message.Gender : user.Gender;
            user.BirthDate = !string.IsNullOrEmpty(message.BirthDate.ToString()) ? message.BirthDate : user.BirthDate;
            user.Nationality = !string.IsNullOrEmpty(message.Nationality) ? message.Nationality : user.Nationality;
            
            await repository.UpdateAsync(user);
        }
    }
}