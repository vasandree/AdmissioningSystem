using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class EducationDocumentInfoResponse(EducationDocumentDto? educationDocumentInfo,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{
   public EducationDocumentDto? EducationDocument { get; set; } = educationDocumentInfo;
}