using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class AddApplicantRoleMessage(Guid userId)
{
    [Required] public Guid UserId { get; set; } = userId;
}