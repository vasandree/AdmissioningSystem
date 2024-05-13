using Common.Models;
using EasyNetQ;
using MediatR;
using UserService.Application.Contracts.Persistence;

namespace UserService.Application.Features.Commands.ProfileCommands.SendEmailCode;

public class SendEmailCodeHandler : IRequestHandler<SendEmailCode, Unit>
{
    private readonly IUserRepository _repository;
    private readonly IBus _bus;

    public SendEmailCodeHandler(IUserRepository repository, IBus bus)
    {
        _repository = repository;
        _bus = bus;
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

            var confirmCode = GenerateConfirmCode();
            user.ConfirmCode = confirmCode;
            await _repository.UpdateAsync(user);

            var notificationMessage = new ForgetPasswordMessage
            {
                Email = request.Email,
                ConfirmCode = confirmCode
            };
            await _bus.PubSub.PublishAsync(notificationMessage, cancellationToken: cancellationToken);

            return Unit.Value;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            throw; 
        }
    }

    private string GenerateConfirmCode()
    {
        Random rand = new Random();
        int randomNumber = rand.Next(1, 999999);
        string code = randomNumber.ToString("D6");
        return code;
    }
}