using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class EducationDocumentTypeCheckResponse(bool exists)
{
    [Required] public bool Exists { get; set; } = exists;
}