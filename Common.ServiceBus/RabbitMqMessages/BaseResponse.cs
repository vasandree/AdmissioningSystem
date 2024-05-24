namespace Common.ServiceBus.RabbitMqMessages;

public class BaseResponse(Exception? exceptionToThrow = null)
{
    public Exception? ExceptionToThrow { get; set; } = exceptionToThrow;
}