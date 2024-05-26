using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.ServiceBus.PubSub.Listeners;

public class AddApplicantRoleListener : BackgroundService
{
    private readonly IBus _bus;
    private IServiceProvider _serviceProvider;

    public AddApplicantRoleListener(IBus bus)
    {
        _bus = bus;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _bus.PubSub.SubscribeAsync<AddApplicantRoleMessage>("add_applicant_role_id", AddRole);
    }

    private async Task AddRole(AddApplicantRoleMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var applicantRepository = scope.ServiceProvider.GetRequiredService<IApplicantRepository>();
                
            var user = await repository.GetById(message.UserId);
            await userManager.AddToRoleAsync(user, "Applicant");
            await applicantRepository.CreateAsync(new ApplicantEntity
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                User = user
            });

        }
    }
}