using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UploadNewFileMessage(Guid userId, DocumentType documentType, IFormFile file)
{
    [Required] public Guid UserId { get; set; } = userId;
    [Required] public DocumentType DocumentType { get; set; } = documentType;
    [Required] public IFormFile File { get; set; } = file;
}