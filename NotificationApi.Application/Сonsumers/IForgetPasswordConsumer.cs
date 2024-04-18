namespace NotificationApi.Application.Сonsumers;

public interface IForgetPasswordConsumer
{
    Task StartConsuming(CancellationToken stoppingToken);
}