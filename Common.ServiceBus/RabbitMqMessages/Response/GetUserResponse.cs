using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetUserResponse( Exception? exception = null) : BaseResponse(exception)
{
    [Required] public Guid UserId { get; set; }
    [Required] public string FullName { get; set; }
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}