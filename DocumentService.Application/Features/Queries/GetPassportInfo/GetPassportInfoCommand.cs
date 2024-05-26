using Common.Models.Models.Dtos;
using MediatR;

namespace DocumentService.Application.Features.Queries.GetPassportInfo;

public record GetPassportInfoCommand(Guid UserId) : IRequest<PassportInfoDto>;