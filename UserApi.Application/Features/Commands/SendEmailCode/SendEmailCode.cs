using MediatR;

namespace UserApi.Application.Features.Commands.SendEmailCode;

public record SendEmailCode(string Email) : IRequest<Unit>;