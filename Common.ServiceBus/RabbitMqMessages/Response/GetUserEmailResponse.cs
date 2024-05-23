using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetUserEmailResponse(string email)
{
    [Required] public string Email { get; set; } = email;
}