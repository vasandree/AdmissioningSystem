using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetEducationDocumentTypeRequest(Guid documentId)
{
    [Required] public Guid DocumentTypeId { get; set; } = documentId;
}