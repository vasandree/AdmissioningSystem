using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdateEducationDocMessage(Guid userId, string newName)
{
    [Required] public Guid UserId { get; set; } = userId;
    [Required] public string Name { get; set; } = newName;
}