using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Enums;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class CheckDocumentExistenceRequest(Guid userId, DocumentType documentType, bool checkFile)
{
    [Required] public Guid UserId { get; set; } = userId;
    [Required] public DocumentType DocumentType { get; set; } = documentType;
    public bool CheckFile { get; set; } = checkFile;
}
