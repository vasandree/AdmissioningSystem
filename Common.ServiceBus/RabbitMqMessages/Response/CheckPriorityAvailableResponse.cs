namespace Common.ServiceBus.RabbitMqMessages.Response;

public class CheckPriorityAvailableResponse(bool closed,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    public bool Closed { get; set; } = closed;
}