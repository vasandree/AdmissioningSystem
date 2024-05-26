using Common.Models.Models.Enums;
using Common.ServiceBus.RabbitMqMessages;
using Common.ServiceBus.RabbitMqMessages.Request;
using Common.ServiceBus.RabbitMqMessages.Response;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Features.Queries.DownloadFile;
using DocumentService.Application.Features.Queries.GetEducationDocumentInfo;
using DocumentService.Application.Features.Queries.GetPassportInfo;
using DocumentService.Domain.Entities;
using EasyNetQ;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentService.Application.ServiceBus.RPC.RpcHandler;

public class RpcRequestHandler : BaseRpcHandler
{
    private readonly IBus _bus;
    private readonly IServiceProvider _serviceProvider;

    public RpcRequestHandler(IBus bus, IServiceProvider serviceProvider)
    {
        _bus = bus;
        _serviceProvider = serviceProvider;
    }

    public override void CreateRequestListeners()
    {
        _bus.Rpc.RespondAsync<EducationDocumentRequest, EducationDocumentResponse>(async (request) =>
            HandleException(await CheckIfApplicantHasDocument(request.ApplicantId)));

        _bus.Rpc.RespondAsync<CheckDocumentExistenceRequest, CheckDocumentExistenceResponse>(async (request) =>
            HandleException(await CheckDocumentExistence(request)));

        _bus.Rpc.RespondAsync<EducationDocumentRequest, EducationDocumentInfoResponse>(async (request) =>
            HandleException(await GetEducationDocument(request.ApplicantId)));

        _bus.Rpc.RespondAsync<GetFileRequest, GetFileResponse>(async (request) =>
            HandleException(await GetFile(request)));

        _bus.Rpc.RespondAsync<GetPassportRequest, GetPassportResponse>(async (request) =>
            HandleException(await GetPassport(request)));
    }

    private async Task<GetPassportResponse> GetPassport(GetPassportRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

            return new GetPassportResponse(
                await mediatr.Send(new GetPassportInfoCommand(request.ApplicantId)));
        }
    }

    private async Task<GetFileResponse> GetFile(GetFileRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

            return new GetFileResponse(
                await mediatr.Send(new DownloadFileCommand(request.DocumentType, request.ApplicantId)));
        }
    }

    private async Task<EducationDocumentInfoResponse> GetEducationDocument(Guid applicantId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var mediatr = scope.ServiceProvider.GetRequiredService<IMediator>();

            return new EducationDocumentInfoResponse(
                await mediatr.Send(new GetEducationDocumentInfoCommand(applicantId)));
        }
    }

    private async Task<CheckDocumentExistenceResponse> CheckDocumentExistence(CheckDocumentExistenceRequest request)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            switch (request.DocumentType)
            {
                case DocumentType.Passport:
                    var passport = scope.ServiceProvider.GetRequiredService<IDocumentRepository<Passport>>();
                    if (request.CheckFile)
                    {
                        var entity = await passport.GetByUserId(request.UserId);
                        return new CheckDocumentExistenceResponse(entity.File != null);
                    }

                    return new CheckDocumentExistenceResponse(await passport.CheckExistence(request.UserId));

                case DocumentType.EducationDocument:
                    var educationDoc =
                        scope.ServiceProvider.GetRequiredService<IDocumentRepository<EducationDocument>>();
                    if (request.CheckFile)
                    {
                        var entity = await educationDoc.GetByUserId(request.UserId);
                        return new CheckDocumentExistenceResponse(entity.File != null);
                    }

                    return new CheckDocumentExistenceResponse(await educationDoc.CheckExistence(request.UserId));
            }
        }

        return new CheckDocumentExistenceResponse(false);
    }

    private async Task<EducationDocumentResponse> CheckIfApplicantHasDocument(Guid applicantId)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var educationDocRepository =
                scope.ServiceProvider.GetRequiredService<IDocumentRepository<EducationDocument>>();
            if (await educationDocRepository.CheckExistence(applicantId))
            {
                var document = (EducationDocument)await educationDocRepository.GetByUserId(applicantId);

                return new EducationDocumentResponse(document.EducationDocumentTypeId);
            }

            return new EducationDocumentResponse(null);
        }
    }
}