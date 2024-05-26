namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetAllApplicantsRequest(string role)
{
    private string Role { get; set; } = role;
}