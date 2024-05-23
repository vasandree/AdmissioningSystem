using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class UpdateDocumentTypeMessage(Guid educationTypeId)
{
    [Required] public Guid EducationTypeId { get; set; } = educationTypeId;
}