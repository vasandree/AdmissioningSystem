using Common.Models.Models.Enums;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetAllAdmissionsRequest(
    Guid[]? faculties,
    Guid? program,
    AdmissionStatus? status,
    bool my,
    Guid managerId,
    string? name,
    int size,
    int page,
    bool noManager)
{
    public Guid[]? Faculties { get; set; } = faculties;
    public Guid? Program { get; set; } = program;
    public AdmissionStatus? Status { get; set; } = status;
    public Guid? ManagerId { get; set; } = my ? managerId : null;
    public string? Name { get; set; } = name;
    public int Size { get; set; } = size;
    public int Page { get; set; } = page;
    public bool NoManager { get; set; } = noManager;
}