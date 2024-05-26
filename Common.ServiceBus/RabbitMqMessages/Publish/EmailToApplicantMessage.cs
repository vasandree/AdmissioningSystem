using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class EmailToApplicantMessage(string email, Guid admissionId)
{
    [Required] public string Email { get; set; } = email;
    [Required] public Guid AdmissionId { get; set; } = admissionId;
}