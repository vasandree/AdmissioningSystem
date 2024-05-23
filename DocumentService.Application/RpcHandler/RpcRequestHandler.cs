using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using EasyNetQ;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Application.RpcHandler;

public class RpcRequestHandler
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public RpcRequestHandler(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    public  void CreateRequestListeners()
    { 
        _bus.Rpc.RespondAsync<EducationDocumentRequest,EducationDocumentResponse >(async (request) =>
            await CheckIfApplicantHasDocument(request.ApplicantId));
    }

    private async Task<EducationDocumentResponse> CheckIfApplicantHasDocument(Guid applicantId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var educationDocRepository = scope.ServiceProvider.GetRequiredService<IDocumentRepository<EducationDocument>>();
            if (await educationDocRepository.CheckExistence(applicantId))
            {
                var document = await educationDocRepository.GetByUserId(applicantId);

                return new EducationDocumentResponse(document.Id);
            }

            return new EducationDocumentResponse(null);
        }
            
    }
}