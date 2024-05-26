using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Application.ServiceBus.PubSub.Sender;
using DocumentService.Application.ServiceBus.RPC.RpcSender;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.EditEducationDocumentInfo;

public class EditEducationDocumentInfoCommandHandler : IRequestHandler<EditEducationDocumentInfoCommand, Unit>
{
    private readonly IDocumentRepository<EducationDocument> _eduacationDocument;
    private readonly RpcRequestSender _rpc;
    private readonly PubSubSender _pubSub;

    public EditEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> eduacationDocument,
        RpcRequestSender rpc, PubSubSender pubSub)
    {
        _eduacationDocument = eduacationDocument;
        _rpc = rpc;
        _pubSub = pubSub;
    }


    public async Task<Unit> Handle(EditEducationDocumentInfoCommand request, CancellationToken cancellationToken)
    {
        if (await _rpc.CheckStatusClosed(request.UserId))
            throw new BadRequest("You cannot edit your profile, because one of your admissions is closed");


        if (!await _eduacationDocument.CheckExistence(request.UserId))
        {
            throw new BadRequest("You haven't added education document yet");
        }

        var educationDocument = (EducationDocument)(await _eduacationDocument.GetByUserId(request.UserId))!;

        if (educationDocument.EducationDocumentTypeId == null || educationDocument.Name == null)
        {
            throw new BadRequest("You haven't added education document info yet");
        }

        var documentExists =
            await _rpc.CheckIfDocumentTypeExists(request.EducationDocumentRequest.EducationDocumentTypeId);
        if (documentExists.Exists == false)
            throw new BadRequest(
                $"DocumentType with {request.EducationDocumentRequest.EducationDocumentTypeId} does not exist");

        educationDocument.EducationDocumentTypeId = request.EducationDocumentRequest.EducationDocumentTypeId;
        educationDocument.Name = request.EducationDocumentRequest.Name;

        await _eduacationDocument.UpdateAsync(educationDocument);

        _pubSub.UpdateStatus(request.UserId);

        
        return Unit.Value;
    }
}