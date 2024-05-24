using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class EducationDocumentTypeCheckResponse(bool exists, Exception? exceptionToThrow = null)
    : BaseResponse(exceptionToThrow)
{
    public bool Exists { get; set; } = exists;
}