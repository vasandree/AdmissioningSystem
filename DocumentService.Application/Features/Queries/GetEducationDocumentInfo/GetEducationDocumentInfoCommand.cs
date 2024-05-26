using Common.Models.Models.Dtos;
using MediatR;

namespace DocumentService.Application.Features.Queries.GetEducationDocumentInfo;

public record GetEducationDocumentInfoCommand(Guid UserId): IRequest<EducationDocumentDto>;