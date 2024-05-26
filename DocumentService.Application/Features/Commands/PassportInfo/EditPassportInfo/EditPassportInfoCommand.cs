using Common.Models.Models.Dtos;
using DocumentService.Application.Dtos.Requests;
using MediatR;

namespace DocumentService.Application.Features.Commands.PassportInfo.EditPassportInfo;

public record EditPassportInfoCommand(Guid UserId, PassportInfoRequest PassportInfoRequest) : IRequest<Unit>;