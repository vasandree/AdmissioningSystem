using DocumentService.Domain.Enums;
using MediatR;

namespace DocumentService.Application.Features.Commands.DeleteDocument;

public record DeleteDocumentCommand(DocumentType DocumentType, Guid Id) : IRequest<Unit>;