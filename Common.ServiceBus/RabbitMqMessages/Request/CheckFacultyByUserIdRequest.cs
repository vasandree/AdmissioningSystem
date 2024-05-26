using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class CheckFacultyByUserIdRequest(Guid userId, Guid facultyId)
{
    [Required] public Guid UserId { get; set; } = userId;
    [Required] public Guid FacultyId { get; set; } = facultyId;
}