using Common.Models.Models.Enums;
using MediatR;

namespace AdminPanel.Application.Features.Commands.Users.UserFiles.DeleteFile;

public record DeleteUserFileCommand(Guid ManagerId, Guid UserId, DocumentType DocumentType) : IRequest<Unit>;