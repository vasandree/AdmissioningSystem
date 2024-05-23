using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetUserEmailRequest(Guid userId)
{
    [Required] public Guid UserId { get; set; } = userId;
}