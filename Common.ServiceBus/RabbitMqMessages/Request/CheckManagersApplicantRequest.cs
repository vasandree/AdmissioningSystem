using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class CheckManagersApplicantRequest(Guid managerId, Guid applicantId)
{
    [Required] public Guid ManagerId { get; set; } = managerId;
    [Required] public Guid ApplicantId { get; set; } = applicantId;
}