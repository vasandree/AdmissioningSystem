using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetAllApplicantAdmissionsRequest(Guid userId)
{
    [Required] public Guid ApplicantId { get; set; } = userId;
}