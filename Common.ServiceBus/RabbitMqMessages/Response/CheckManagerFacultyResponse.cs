namespace Common.ServiceBus.RabbitMqMessages.Response;

public class CheckManagerFacultyResponse(bool valid,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    public bool Valid { get; set; } = valid;
}