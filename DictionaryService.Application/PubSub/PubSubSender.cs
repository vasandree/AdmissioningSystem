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


    public async Task SoftDeleteDocuments(List<DocumentType> documentsToDelete)
    {
        var documentsToDeleteIds = documentsToDelete.Select(doc => doc.Id).ToList();
        await _bus.PubSub.PublishAsync(new DocumentsToDeleteMessage(documentsToDeleteIds));
        //todo: check
    }

    public async Task SoftDeletePrograms(List<Program> programsToDelete)
    {
        var programsToDeleteIds = programsToDelete.Select(program => program.Id).ToList();
        await _bus.PubSub.PublishAsync(new ProgramsToDeleteMessage(programsToDeleteIds));
        //todo: check
    }

    public async Task UpdateDocumentType(DocumentType existingDocumentType)
    {
        await _bus.PubSub.PublishAsync(new UpdateDocumentTypeMessage(existingDocumentType.Id));
        //todo: check
    }

    public async Task UpdateProgram(Program existingProgram)
    {
        await _bus.PubSub.PublishAsync(new ProgramToUpdateMessage(existingProgram.Id));
        //todo: check
    }
    

    public async Task UpdateDocumentTypesByEducationLevel(List<Guid> documentTypesIds)
    {
        foreach (var doc in documentTypesIds)
        {
            await _bus.PubSub.PublishAsync(new UpdateDocumentTypeMessage(doc));
            //todo: check
        }
    }

    public async Task UpdatePrograms(IReadOnlyList<Program> programs)
    {
        foreach (var program in programs)
        {
            await _bus.PubSub.PublishAsync(new ProgramToUpdateMessage(program.Id));
            //todo: check
        }
    }
}