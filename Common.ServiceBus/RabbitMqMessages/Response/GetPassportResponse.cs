using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetPassportResponse(PassportInfoDto passportInfoDto, Exception? exception = null)
    : BaseResponse(exception)
{
    public PassportInfoDto PassportInfoDto { get; set; } = passportInfoDto;
}