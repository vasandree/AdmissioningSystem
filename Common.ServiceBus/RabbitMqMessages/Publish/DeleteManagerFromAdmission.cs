using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdateManagerFromAdmissionMessage(Guid admissionId, Guid? mangerId = null)
{
    [Required] public Guid AdmissionId { get; set; } = admissionId;
    public Guid? ManagerId { get; set; } = mangerId;
}