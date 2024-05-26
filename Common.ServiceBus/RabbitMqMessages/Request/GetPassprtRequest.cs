using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetPassportRequest (Guid applicantId)
{
    [Required] public Guid ApplicantId { get; set; } = applicantId;
}