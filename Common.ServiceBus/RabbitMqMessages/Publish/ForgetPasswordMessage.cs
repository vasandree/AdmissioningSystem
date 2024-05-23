namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class ForgetPasswordMessage
{
    public required string Email { get; set; }
    public required string ConfirmCode { get; set; }
}