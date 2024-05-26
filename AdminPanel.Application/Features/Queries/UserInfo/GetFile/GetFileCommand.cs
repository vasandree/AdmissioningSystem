using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Queries.UserInfo.GetFile;

public record GetFileCommand(Guid ApplicantId, DocumentType DocumentType) : IRequest<Unit>;