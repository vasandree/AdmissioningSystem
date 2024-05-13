using MediatR;
using UserService.Application.Dtos.Requests;

namespace UserService.Application.Features.Commands.ProfileCommands.EditPassword;

public record EditPasswordCommand(Guid UserId, PasswordChangeDto Dto) : IRequest<Unit>;