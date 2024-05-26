using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class CheckNotMyAdmissionRequest(Guid admissionId, Guid managerId)
{
    [Required] public Guid AdmissionId { get; set; } = admissionId;
    [Required] public Guid ManagerId { get; set; } = managerId;
}