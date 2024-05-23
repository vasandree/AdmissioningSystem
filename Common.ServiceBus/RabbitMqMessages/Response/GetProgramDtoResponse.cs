using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetProgramDtoResponse(ProgramDto? programDto)
{
    public ProgramDto? ProgramDto { get; set; } = programDto;
}