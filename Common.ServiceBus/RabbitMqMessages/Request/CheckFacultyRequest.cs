using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class CheckFacultyRequest(Guid facultyId)
{
    [Required] public Guid FacultyId { get; set; } = facultyId;
}