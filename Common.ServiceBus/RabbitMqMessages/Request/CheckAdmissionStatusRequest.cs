using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class CheckAdmissionStatusRequest(Guid admissionId)
{
    [Required] public Guid AdmissionId { get; set; } = admissionId;
}