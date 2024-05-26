using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetProgramIdsResponse(List<Guid> programIds, Exception? exception = null) : BaseResponse(exception)
{
    [Required] public List<Guid> ProgramIds { get; set; } = programIds;
}