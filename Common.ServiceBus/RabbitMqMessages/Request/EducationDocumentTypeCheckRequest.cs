using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class EducationDocumentTypeCheckRequest(Guid documentId)
{
    [Required] public Guid DocumentTypeId { get; set; } = documentId;
}