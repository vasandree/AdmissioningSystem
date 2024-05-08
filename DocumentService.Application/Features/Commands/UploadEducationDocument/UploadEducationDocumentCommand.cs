using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Commands.UploadEducationDocument;

public record UploadEducationDocumentCommand(IFormFile File, Guid UserId, Guid DocumentTypeId, string Name ) : IRequest<Unit>;