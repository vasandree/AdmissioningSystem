using System.Text;
using Common.Models;
using Newtonsoft.Json;
using RabbitMQ.Client;
using UserApi.Application.Contracts.Publishers;

namespace UserApi.Persistence.Publishers;

public class ForgetPasswordPublisher : IForgetPasswordPublisher
{
    public void PublishMessageToRabbitMq(ForgetPasswordMessage forgetPassword)
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
 
        var message = JsonConvert.SerializeObject(forgetPassword);
        var body = Encoding.UTF8.GetBytes(message);
 
        channel.BasicPublish(exchange: "",
            routingKey: "email_queue",
            basicProperties: null,
            body: body);
    }
}