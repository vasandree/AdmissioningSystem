using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetFacultyResponse(FacultyDto? faculty, Exception? exception = null)
    : BaseResponse(exception)
{
    public FacultyDto? Faculty { get; set; } = faculty;
}