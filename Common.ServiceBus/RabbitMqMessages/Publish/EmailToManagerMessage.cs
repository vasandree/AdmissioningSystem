using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class EmailToManagerMessage(string managerEmail, Guid admissionId)
{
    [Required] public string Email { get; set; } = managerEmail;
    [Required] public Guid AdmissionId { get; set; } = admissionId;
}