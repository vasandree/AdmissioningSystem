using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetProgramIdsRequest(Guid[] facultyIds)
{
    [Required] public Guid[] FacultyIds { get; set; } = facultyIds;
}