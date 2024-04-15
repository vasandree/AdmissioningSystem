using MediatR;
using UserApi.Application.Dtos.Requests;

namespace UserApi.Application.Features.Commands.EditPassword;

public record EditPasswordCommand(string Email, PasswordChangeDto Dto) : IRequest;