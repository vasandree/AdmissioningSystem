using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class GetApplicantMessage(Guid admissionId)
{
    [Required] public Guid AdmissionId { get; set; } = admissionId;
}