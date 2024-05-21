using DocumentService.Application.Dtos.Requests;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.AddEducationDocumentInfo;

public record AddEducationDocumentInfoCommand(Guid UserId, EducationDocumentRequest EducationDocumentRequest): IRequest<Unit>;