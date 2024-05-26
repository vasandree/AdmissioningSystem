using Common.Models.Models.Dtos;
using DocumentService.Application.Dtos.Requests;
using MediatR;

namespace DocumentService.Application.Features.Commands.PassportInfo.AddPassportInfo;

public record AddPassportInfoCommand(Guid UserId, PassportInfoRequest PassportInfo) : IRequest<Unit>;