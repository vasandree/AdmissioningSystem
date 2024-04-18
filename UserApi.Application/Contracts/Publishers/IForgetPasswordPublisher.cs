using Common.Models;

namespace UserApi.Application.Contracts.Publishers;

public interface IForgetPasswordPublisher
{
    void PublishMessageToRabbitMq(ForgetPasswordMessage forgetPassword);
}