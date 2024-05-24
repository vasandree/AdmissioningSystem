using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using EasyNetQ;

namespace DocumentService.Application.RPC;

public class RpcRequestSender
{
    private readonly IBus _bus;

    public RpcRequestSender(IBus bus)
    {
        _bus = bus;
    }

    public async Task<EducationDocumentTypeCheckResponse> CheckIfDocumentTypeExists(Guid documentId)
    {
        return await _bus.Rpc.RequestAsync<EducationDocumentTypeCheckRequest, EducationDocumentTypeCheckResponse>(
            new EducationDocumentTypeCheckRequest(documentId));
    }

    public async Task<GetDocumentTypeDtoResponse> GetDocumentTypeDto(Guid documentId)
    {
        return await _bus.Rpc.RequestAsync<GetDocumentTypeDtoRequest, GetDocumentTypeDtoResponse>(
            new GetDocumentTypeDtoRequest(documentId));
    }
}