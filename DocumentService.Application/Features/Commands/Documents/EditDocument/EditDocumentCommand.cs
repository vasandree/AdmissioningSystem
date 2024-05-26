using Common.Models.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Commands.Documents.EditDocument;

public record EditDocumentCommand(DocumentType DocumentType, IFormFile File, Guid Id) : IRequest<Unit>;