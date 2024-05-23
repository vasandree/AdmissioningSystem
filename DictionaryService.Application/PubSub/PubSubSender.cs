using Common.ServiceBus.RabbitMqMessages.Publish;
using DictionaryService.Application.Contracts.Persistence;
using DictionaryService.Domain.Entities;
using EasyNetQ;

namespace DictionaryService.Application.PubSub;

public class PubSubSender
{
    private readonly IBus _bus;

    public PubSubSender(IBus bus)
    {
        _bus = bus;
    }


    public void SoftDeleteDocuments(List<DocumentType> documentsToDelete)
    {
        var documentsToDeleteIds = documentsToDelete.Select(doc => doc.Id).ToList();
        _bus.PubSub.Publish(new DocumentsToDeleteMessage(documentsToDeleteIds));
    }

    public void SoftDeletePrograms(List<Program> programsToDelete)
    {
        var programsToDeleteIds = programsToDelete.Select(program => program.Id).ToList();
        _bus.PubSub.Publish(new ProgramsToDeleteMessage(programsToDeleteIds));
    }

    public void UpdateDocumentType(DocumentType existingDocumentType)
    {
        _bus.PubSub.Publish(new UpdateDocumentTypeMessage(existingDocumentType.Id));
    }

    public void UpdateProgram(Program existingProgram)
    {
        _bus.PubSub.Publish(new ProgramToUpdateMessage(existingProgram.Id));
    }
    

    public void UpdateDocumentTypesByEducationLevel(List<Guid> documentTypesIds)
    {
        foreach (var doc in documentTypesIds)
        {
            _bus.PubSub.Publish(new UpdateDocumentTypeMessage(doc));
        }
    }

    public void UpdatePrograms(IReadOnlyList<Program> programs)
    {
        foreach (var program in programs)
        {
            _bus.PubSub.Publish(new ProgramToUpdateMessage(program.Id));
        }
    }
}