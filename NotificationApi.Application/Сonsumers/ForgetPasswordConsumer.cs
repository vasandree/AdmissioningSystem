using System.Text;
using Common.Models;
using EasyNetQ;
using Newtonsoft.Json;
using NotificationApi.Application.Contracts;
using NotificationApi.Application.Models;
using RabbitMQ.Client;

namespace NotificationApi.Application.Сonsumers;

public class ForgetPasswordConsumer : IForgetPasswordConsumer
{
    private readonly IEmailSender _sender;

    public ForgetPasswordConsumer(IEmailSender sender)
    {
        _sender = sender;
    }

    public async Task StartConsuming(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5672,
            UserName = "guest",
            Password = "guest"
        };
 
        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();
 
        channel.QueueDeclare(queue: "email_queue",
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);
 
        while (!stoppingToken.IsCancellationRequested)
        {
            var eaGetResult = await Task.Run(() => channel.BasicGet(queue: "email_queue", autoAck: false));
 
            if (eaGetResult != null)
            {
                var body = eaGetResult.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var emailData = JsonConvert.DeserializeObject<ForgetPasswordMessage>(message);

                var confirmCode = GenerateRandomCode();
                var emailMessage = new EmailMessage
                {
                    To = emailData.Email,
                    Subject = "Forget password code",
                    Body = $"Your code: {confirmCode}"
                };

                await _sender.SendEmail(emailMessage);
 
                channel.BasicAck(deliveryTag: eaGetResult.DeliveryTag, multiple: false);
            }
 
            await Task.Delay(1000, stoppingToken);
        }
    }


    private string GenerateRandomCode()
    {
        Random rand = new Random();
        int randomNumber = rand.Next(1, 999999);
        string code = randomNumber.ToString("D6");
        return code;
    }
    
}