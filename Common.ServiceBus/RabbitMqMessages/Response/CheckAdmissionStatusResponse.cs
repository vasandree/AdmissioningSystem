namespace Common.ServiceBus.RabbitMqMessages.Response;

public class CheckAdmissionStatusResponse(bool closed,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    public bool Closed { get; set; } = closed;
}