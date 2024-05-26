using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Enums;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetFileRequest(Guid applicantId, DocumentType documentType)
{
    [Required] public Guid ApplicantId { get; set; } = applicantId;
    [Required] public DocumentType DocumentType { get; set; } = documentType;
}