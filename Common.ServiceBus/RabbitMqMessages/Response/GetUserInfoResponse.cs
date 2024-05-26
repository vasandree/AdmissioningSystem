using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetUserInfoResponse(UserDto userDto,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{
    public UserDto UserInfo { get; set; } = userDto;
}