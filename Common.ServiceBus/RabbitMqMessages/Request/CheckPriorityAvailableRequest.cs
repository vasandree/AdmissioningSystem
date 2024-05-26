using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class CheckPriorityAvailableRequest(Guid admissionId, int newPriority)
{
    [Required] public Guid AdmissionId { get; set; } = admissionId;
    [Required] public int NewPriority { get; set; } = newPriority;
}