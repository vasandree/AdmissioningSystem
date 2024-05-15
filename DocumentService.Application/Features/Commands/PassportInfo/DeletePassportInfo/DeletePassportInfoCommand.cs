using MediatR;

namespace DocumentService.Application.Features.Commands.PassportInfo.DeletePassportInfo;

public record DeletePassportInfoCommand(Guid UserId) : IRequest<Unit>;