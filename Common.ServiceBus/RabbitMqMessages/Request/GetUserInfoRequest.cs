using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetUserInfoRequest(Guid applicantId)
{
    [Required] public Guid ApplicantId { get; set; } = applicantId;
}