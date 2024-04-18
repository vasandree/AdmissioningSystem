using Common.Models;
using MediatR;
using UserApi.Application.Contracts.Persistence;
using UserApi.Application.Contracts.Publishers;

namespace UserApi.Application.Features.Commands.SendEmailCode;

public class SendEmailCodeHandler : IRequestHandler<SendEmailCode, Unit>
{
    private readonly IUserRepository _repository;
    private readonly IForgetPasswordPublisher _publisher;

    public SendEmailCodeHandler(IUserRepository repository,IForgetPasswordPublisher publisher)
    {
        _repository = repository;
        _publisher = publisher;
    }

    public async Task<Unit> Handle(SendEmailCode request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _repository.GetByEmail(request.Email);
            if (user == null)
            {
                throw new Exception("User not found.");
            }
            
            _publisher.PublishMessageToRabbitMq(new ForgetPasswordMessage() { Email = user.Email });

            return Unit.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw; 
        }
    }
}