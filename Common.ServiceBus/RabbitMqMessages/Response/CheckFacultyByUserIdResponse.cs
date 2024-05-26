namespace Common.ServiceBus.RabbitMqMessages.Response;

public class CheckFacultyByUserIdResponse(bool available,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    public bool Available { get; set; } = available;
}
