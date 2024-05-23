using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class AdminRoleCheckResponse(bool isInRole)
{
    [Required] public bool IsInRole { get; set; } = isInRole;
}