using System.ComponentModel.DataAnnotations;

namespace Common.ServiceBus.RabbitMqMessages.Publish;

public class DocumentsToDeleteMessage(List<Guid> documentIdsToDelete)
{
    [Required] public List<Guid> DocumentIdsToDelete { get; set; } = documentIdsToDelete;
}