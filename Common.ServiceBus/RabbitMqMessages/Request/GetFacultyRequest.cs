using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetFacultyRequest(Guid facultyId)
{
    [Required] public Guid FacultyId { get; set; } = facultyId;
}