using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class AdminRoleCheckRequest(Guid userId, string requiredRole)
{
    [Required]
    public Guid UserId { get; set; } = userId;

    [Required]
    public string RequiredRole { get; set; } = requiredRole;
}