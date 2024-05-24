using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetEducationDocumentResponse(EducationDocumentTypeDto? documentTypeDto, Exception? exception = null)
    : BaseResponse(exception)
{
    public EducationDocumentTypeDto? EducationDocumentTypeDto { get; set; } = documentTypeDto;
}