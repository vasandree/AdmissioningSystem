using Common.Models.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Commands.Documents.UploadDocument;

public record UploadDocumentRequest(DocumentType DocumentType, IFormFile File, Guid Id) : IRequest<Unit>;