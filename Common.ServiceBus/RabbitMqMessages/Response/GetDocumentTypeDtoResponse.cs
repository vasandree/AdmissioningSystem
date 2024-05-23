using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetDocumentTypeDtoResponse(EducationDocumentTypeDto? documentTypeDto)
{
    public EducationDocumentTypeDto? EducationDocumentTypeDto { get; set; } = documentTypeDto;
}