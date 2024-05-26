using Microsoft.AspNetCore.Http;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetFileResponse((byte[], string, string) file, Exception? exception = null)
    : BaseResponse(exception)
{
    public (byte[], string, string)? File { get; set; } = file;
}