using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DictionaryService.Application.Contracts.Persistence;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;

namespace DictionaryService.Application.RpcHandler;

public class ExistanceInDictionaryRpcHandler
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public ExistanceInDictionaryRpcHandler(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    public void CreateRequestListeners()
    {
        _bus.Rpc.RespondAsync<EducationDocumentTypeCheckRequest,EducationDocumentTypeCheckResponse >(async (request) =>
            await CheckIfDocumentTypeExists(request.DocumentTypeId));
    }

    private async Task<EducationDocumentTypeCheckResponse> CheckIfDocumentTypeExists(Guid documentId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var educationDoc = scope.ServiceProvider.GetRequiredService<IDocumentTypeRepository>();
            var exists = await educationDoc.CheckExistenceById(documentId);

            return new EducationDocumentTypeCheckResponse(exists);

        }
    }
}