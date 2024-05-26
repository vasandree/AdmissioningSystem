using Common.Models.Exceptions;
using Common.Models.Models.Enums;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Dtos.Requests;
using DocumentService.Application.Helpers;
using DocumentService.Application.ServiceBus.PubSub.Sender;
using DocumentService.Application.ServiceBus.RPC.RpcSender;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.AddEducationDocumentInfo;

public class AddEducationDocumentInfoCommandHandler : IRequestHandler<AddEducationDocumentInfoCommand, Unit>
{
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly PubSubSender _pubSub;
    private readonly RpcRequestSender _rpc;

    public AddEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> educationDocument,
        RpcRequestSender rpc, PubSubSender pubSub)
    {
        _educationDocument = educationDocument;
        _rpc = rpc;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(AddEducationDocumentInfoCommand request, CancellationToken cancellationToken)
    {
        if (await _rpc.CheckStatusClosed(request.UserId))
            throw new BadRequest("You cannot edit your profile, because one of your admissions is closed");

        
        if (await _educationDocument.CheckExistence(request.UserId))
        {
            await ModifyEntity(request.UserId, request.EducationDocumentRequest);
        }
        else
        {
            await CreateNewEntity(request.UserId, request.EducationDocumentRequest);
        }

        _pubSub.UpdateStatus(request.UserId);
        
        return Unit.Value;
    }


    private async Task CreateNewEntity(Guid userId, EducationDocumentRequest educationDocumentRequest)
    {
        if (await _educationDocument.CheckExistence(userId))
        {
            var document = (EducationDocument)await _educationDocument.GetByUserId(userId);
            if (document.IsDeleted || document.EducationDocumentTypeId != null || document.Name != null)
            {
                throw new Conflict("You have already added education document info");
            }
        }

        var documentExists = await _rpc.CheckIfDocumentTypeExists(educationDocumentRequest.EducationDocumentTypeId);

        if (documentExists.Exists == false)
            throw new BadRequest(
                $"DocumentType with {educationDocumentRequest.EducationDocumentTypeId} does not exist");

        await _educationDocument.CreateAsync(new EducationDocument
        {
            Id = Guid.NewGuid(),
            DocumentType = DocumentType.Passport,
            UserId = userId,
            File = null,
            EducationDocumentTypeId = educationDocumentRequest.EducationDocumentTypeId,
            Name = educationDocumentRequest.Name
        });
    }

    private async Task ModifyEntity(Guid userId, EducationDocumentRequest educationDocumentRequest)
    {
        var document = (EducationDocument)await _educationDocument.GetByUserId(userId);
        if (document.IsDeleted || document.EducationDocumentTypeId != null || document.Name != null)
        {
            throw new Conflict("You have already added education document info");
        }

        var educationDocument = (EducationDocument)(await _educationDocument.GetByUserId(userId))!;

        var documentExists = await _rpc.CheckIfDocumentTypeExists(educationDocumentRequest.EducationDocumentTypeId);
        if (documentExists.Exists == false)
            throw new BadRequest(
                $"DocumentType with {educationDocumentRequest.EducationDocumentTypeId} does not exist");

        educationDocument.EducationDocumentTypeId = educationDocumentRequest.EducationDocumentTypeId;
        educationDocument.Name = educationDocumentRequest.Name;
        
        await _educationDocument.UpdateAsync(educationDocument);
    }
}