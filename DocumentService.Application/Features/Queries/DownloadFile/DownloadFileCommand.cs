using DocumentService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Queries.DownloadFile;

public record DownloadFileCommand(DocumentType DocumentType, Guid UserId) : IRequest<(byte[], string, string)>;
