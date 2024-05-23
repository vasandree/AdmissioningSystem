using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetProgramDtoRequest(Guid programId)
{
    [Required] public Guid ProgramId { get; set; } = programId;
}