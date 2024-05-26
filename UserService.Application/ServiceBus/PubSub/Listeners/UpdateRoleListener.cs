using Common.Models.Exceptions;
using Common.ServiceBus.RabbitMqMessages.Publish;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UserApi.Domain.DbEntities;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.ServiceBus.PubSub.Listeners;

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
        await _bus.PubSub.SubscribeAsync<UpdateUserRoleMessage>("update_role_subscription_id",
            UpdateUserRole);
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
                await DeleteByRole(user, message.Role);
            }
            else
            {
                await repository.DeleteRole(user, message.Role);
                await repository.UpdateAsync(user);
                await CreateByRole(user, message.Role, message.FacultyId);
            }
        }
    }

    private async Task CreateByRole(ApplicationUser user, string role, Guid? facultyId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            if (role == "Applicant")
            {
                var repository = scope.ServiceProvider.GetRequiredService<IApplicantRepository>();
                await repository.CreateAsync(new ApplicantEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    User = user
                });
            }
            else
            {
                var repository = scope.ServiceProvider.GetRequiredService<IManagerRepository>();
                await repository.CreateAsync(new ManagerEntity
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    User = user,
                    Faculty = facultyId
                });
            }
        }
    }

    private async Task DeleteByRole(ApplicationUser user, string role)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            if (role == "Applicant")
            {
                var repository = scope.ServiceProvider.GetRequiredService<IApplicantRepository>();
                await repository.DeleteAsync((await repository.GetByUserId(user.Id))!);
            }
            else
            {
                var repository = scope.ServiceProvider.GetRequiredService<IManagerRepository>();
                await repository.DeleteAsync((await repository.GetById(user.Id))!);
            }
        }
    }
}