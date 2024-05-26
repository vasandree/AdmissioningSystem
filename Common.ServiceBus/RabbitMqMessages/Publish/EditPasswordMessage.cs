using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class EditPasswordMessage(Guid userId, string oldPassword, string newPassword)
{
    [Required] public Guid UserId { get; set; } = userId;
    [Required] public string OldPassword { get; set; } = oldPassword;
    [Required] public string NewPassword { get; set; } = newPassword;
}