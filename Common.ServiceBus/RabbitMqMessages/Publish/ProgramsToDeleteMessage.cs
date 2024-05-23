using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class ProgramsToDeleteMessage(List<Guid> programsToDelete)
{
    [Required] public List<Guid> ProgramsToDelete { get; set; } = programsToDelete;
}