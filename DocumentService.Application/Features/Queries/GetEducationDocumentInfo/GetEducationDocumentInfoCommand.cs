using DocumentService.Application.Dtos.Responses;
using MediatR;

namespace DocumentService.Application.Features.Queries.GetEducationDocumentInfo;

public record GetEducationDocumentInfoCommand(Guid UserId): IRequest<EducationDocumentDto>;