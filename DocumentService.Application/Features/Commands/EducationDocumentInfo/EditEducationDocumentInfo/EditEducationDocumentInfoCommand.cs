using DocumentService.Application.Dtos.Requests;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.EditEducationDocumentInfo;

public record EditEducationDocumentInfoCommand(Guid UserId, EducationDocumentRequest EducationDocumentRequest): IRequest<Unit>;