using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Dtos;

namespace Common.ServiceBus.RabbitMqMessages.Request;

public class GetDocumentTypeDtoRequest(Guid documentTypeId)
{
    [Required]public Guid DocumentTypeId { get; set; } = documentTypeId;
}