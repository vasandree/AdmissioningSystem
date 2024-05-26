using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class GetAllApplicantsResponse(List<UserDto> applicants, Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    public List<UserDto> Applicants { get; set; } = applicants;
}