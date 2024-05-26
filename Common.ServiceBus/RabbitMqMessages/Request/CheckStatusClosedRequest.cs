using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class CheckStatusClosedRequest(Guid userId)
{
    [Required] public Guid UserId { get; set; } = userId;
}