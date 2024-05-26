using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetAllUsersResponse(List<UserDto> users, Exception? exceptionToThrow = null)
    : BaseResponse(exceptionToThrow)
{
    public List<UserDto> Users { get; set; } = users;
}