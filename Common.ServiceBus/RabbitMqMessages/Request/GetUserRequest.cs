using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetUserRequest(Guid userId)
{
    [Required] public Guid UserId { get; set; } = userId;
}