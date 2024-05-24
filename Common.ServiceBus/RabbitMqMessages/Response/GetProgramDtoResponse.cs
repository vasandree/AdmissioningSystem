using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetProgramDtoResponse(ProgramDto? programDto, Exception? exception = null) : BaseResponse(exception)
{
    public ProgramDto? ProgramDto { get; set; } = programDto;
}