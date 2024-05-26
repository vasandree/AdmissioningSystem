using Common.Models.Models.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AdminPanel.Application.Features.Commands.Users.UserFiles.UploadFile;

public record UploadFileCommand(Guid ManagerId, Guid UserId, DocumentType DocumentType, IFormFile File) : IRequest<Unit>;