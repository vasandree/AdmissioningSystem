using DocumentService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Commands.UploadDocument;

public record UploadDocumentRequest(DocumentType DocumentType, IFormFile File, Guid Id) : IRequest<Unit>;