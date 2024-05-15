using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.DeleteEducationDocumentInfo;

public record DeleteEducationDocumentInfoCommand(Guid UserId) : IRequest<Unit>;
