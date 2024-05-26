using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdateUserRoleMessage(Guid userId, string role, Guid? facultyId = null)
{
    [Required] public Guid UserId { get; set; } = userId;
    [Required] public string Role { get; set; } = role;

    public Guid? FacultyId { get; set; } = facultyId;
}