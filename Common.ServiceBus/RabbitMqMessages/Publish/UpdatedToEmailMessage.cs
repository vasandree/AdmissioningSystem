using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdatedToEmailMessage(string email, string message)
{
    [Required] public string Email { get; set; } = email;
    [Required] public string Message { get; set; } = message;
    
}