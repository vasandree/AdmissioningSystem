namespace Common.ServiceBus.RabbitMqMessages.Response;

public class EducationDocumentResponse(Guid? documentTypeId)
{
    public Guid? DocumentTypeId { get; set; } = documentTypeId;
}