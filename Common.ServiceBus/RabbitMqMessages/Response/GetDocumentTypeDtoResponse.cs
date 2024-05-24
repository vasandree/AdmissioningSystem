using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetDocumentTypeDtoResponse(EducationDocumentTypeDto? documentTypeDto, Exception? exception = null)
    : BaseResponse
{
    public EducationDocumentTypeDto? EducationDocumentTypeDto { get; set; } = documentTypeDto;
}