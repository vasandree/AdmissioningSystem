using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetUserEmailResponse(string email, Exception? exception = null) : BaseResponse(exception)
{
    public string Email { get; set; } = email;
}