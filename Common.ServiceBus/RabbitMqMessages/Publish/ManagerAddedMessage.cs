using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class ManagerAddedMessage(string email, string role, Guid? facultyId = null )
{
    [Required] public string Email { get; set; } = email;
    [Required] public string Role { get; set; } = role;
    public Guid? FacultyId { get; set; } = facultyId;
}