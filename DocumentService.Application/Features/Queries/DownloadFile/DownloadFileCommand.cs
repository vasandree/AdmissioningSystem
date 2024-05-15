using DocumentService.Domain.Enums;
using MediatR;

namespace DocumentService.Application.Features.Queries.DownloadFile;

public record DownloadFileCommand(DocumentType DocumentType, Guid UserId) : IRequest<(byte[], string, string)>;
