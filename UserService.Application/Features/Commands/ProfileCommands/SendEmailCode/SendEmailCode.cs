using MediatR;

namespace UserService.Application.Features.Commands.ProfileCommands.SendEmailCode;

public record SendEmailCode(string Email) : IRequest<Unit>;