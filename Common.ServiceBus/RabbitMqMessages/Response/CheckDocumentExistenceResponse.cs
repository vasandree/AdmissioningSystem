namespace Common.ServiceBus.RabbitMqMessages.Response;

public class CheckDocumentExistenceResponse(bool exists,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    public bool Exists { get; set; } = exists;
}