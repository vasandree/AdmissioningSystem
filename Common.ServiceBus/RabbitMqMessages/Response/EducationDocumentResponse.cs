namespace Common.ServiceBus.RabbitMqMessages.Response;

public class EducationDocumentResponse(Guid? documentTypeId, Exception? exception = null) : BaseResponse(exception)
{
    public Guid? DocumentTypeId { get; set; } = documentTypeId;
}