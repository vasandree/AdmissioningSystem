using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class ProgramToUpdateMessage(Guid programId)
{
    [Required] public Guid ProgramId { get; set; } = programId;
}