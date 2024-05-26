using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Enums;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class DeleteFileMessage(Guid userId, DocumentType documentType)
{
    [Required] public Guid UserId { get; set; } = userId;
    [Required] public DocumentType DocumentType { get; set; } = documentType;
}