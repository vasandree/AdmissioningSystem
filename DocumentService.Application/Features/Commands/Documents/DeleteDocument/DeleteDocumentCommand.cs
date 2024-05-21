using DocumentService.Domain.Enums;
using MediatR;

namespace DocumentService.Application.Features.Commands.Documents.DeleteDocument;

public record DeleteDocumentCommand(DocumentType DocumentType, Guid Id) : IRequest<Unit>;