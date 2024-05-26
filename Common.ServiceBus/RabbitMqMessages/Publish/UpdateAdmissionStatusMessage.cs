using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Enums;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdateAdmissionStatusMessage(Guid admissionId, AdmissionStatus status)
{
    [Required] public Guid AdmissionId { get; set; } = admissionId;
    [Required] public AdmissionStatus NewStatus { get; set; } = status;
}