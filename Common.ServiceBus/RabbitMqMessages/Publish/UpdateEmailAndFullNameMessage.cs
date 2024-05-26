using System.ComponentModel.DataAnnotations;
using Common.ServiceBus.RabbitMqMessages.Response;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdateEmailAndFullNameMessage(Guid userId, string fullName, string email)
{
    [Required] public Guid UserId { get; set; } = userId;

    [Required] public string FullName { get; set; } = fullName;

    [Required] public string Email { get; set; } = email;
}