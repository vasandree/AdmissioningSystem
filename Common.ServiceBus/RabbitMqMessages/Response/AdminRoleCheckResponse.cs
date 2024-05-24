using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Response;

public class AdminRoleCheckResponse(bool isInRole,  Exception? exceptionToThrow = null) : BaseResponse(exceptionToThrow)
{ 
    public bool IsInRole { get; set; } = isInRole;
}