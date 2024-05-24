using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.PubSub;

public class UpdateRoleListener : BackgroundService
{
    private IBus _bus;
    private IServiceProvider _serviceProvider;

    public UpdateRoleListener(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _bus.PubSub.Subscribe<UpdateUserRoleMessage>("update_role_subscription_id",
            UpdateUserRole);
        //todo: check
    }

    private async Task UpdateUserRole(UpdateUserRoleMessage message)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var repository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
            var applicantRepository = scope.ServiceProvider.GetRequiredService<IApplicantRepository>();

            var user = await repository.GetById(message.UserId);

            if (user == null)
                throw new NotFound("User not found");

            var existingRoles = await repository.GetUserRoles(user);

            if (existingRoles.Contains(message.Role))
            {
                await repository.AddRole(user, message.Role);
                await repository.UpdateAsync(user);
                await applicantRepository.DeleteAsync((await applicantRepository.GetByUserId(message.UserId))!);
            }
            else
            {
                await repository.DeleteRole(user, message.Role);
                await repository.UpdateAsync(user);
                await applicantRepository.CreateAsync(new ApplicantEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    User = user
                });
            }
        }
    }
}