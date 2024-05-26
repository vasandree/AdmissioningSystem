using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdateAdmissionPriorityMessage (Guid admissionId, int newPriority)
{
    [Required] public Guid AdmissionId { get; set; } = admissionId;
    [Required] public int NewPriority { get; set; } = newPriority;
}