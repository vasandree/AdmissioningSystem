namespace Common.ServiceBus.RabbitMqMessages.Response;

public class CheckNotMyResponse(bool notMy,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    public bool NotMy { get; set; } = notMy;
}