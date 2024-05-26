using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class CheckManagerFacultyRequest(Guid admissionId, Guid facultyId)
{
    [Required] public Guid FacultyId { get; set; } = facultyId;
    [Required] public Guid AdmissionId { get; set; } = admissionId;
}