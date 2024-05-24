using MediatR;

namespace UserService.Application.Features.Commands.ProfileCommands.SendEmailCode;

public record SendEmailCode(Guid Id) : IRequest<Unit>;