namespace Common.ServiceBus.RabbitMqMessages.Response;

public class CheckFacultyResponse(bool exists,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    public bool Exists { get; set; } = exists;
}