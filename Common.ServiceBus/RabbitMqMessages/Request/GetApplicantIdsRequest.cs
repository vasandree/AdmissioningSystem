using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetApplicantIdsRequest(string name)
{
    [Required] public string Name { get; set; } = name;
}