using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.ServiceBus.PubSub.Sender;
using DocumentService.Application.ServiceBus.RPC.RpcSender;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.DeleteEducationDocumentInfo;

public class DeleteEducationDocumentInfoCommandHandler : IRequestHandler<DeleteEducationDocumentInfoCommand, Unit>
{
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly RpcRequestSender _rpc;
    private readonly PubSubSender _pubSub;
    public DeleteEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> educationDocument, RpcRequestSender rpc, PubSubSender pubSub)
    {
        _educationDocument = educationDocument;
        _rpc = rpc;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(DeleteEducationDocumentInfoCommand request, CancellationToken cancellationToken)
    {      
        if (await _rpc.CheckStatusClosed(request.UserId))
            throw new BadRequest("You cannot edit your profile, because one of your admissions is closed");


        
        if (!await _educationDocument.CheckExistence(request.UserId))
        {
            throw new BadRequest("You haven't added education document yet");
        }

        var educationDocument = (EducationDocument)(await _educationDocument.GetByUserId(request.UserId))!;

        if (educationDocument.File == null)
        {
            await _educationDocument.DeleteAsync(educationDocument);
        }
        else
        {
            educationDocument.Name = null;
            educationDocument.EducationDocumentTypeId = null;

            await _educationDocument.UpdateAsync(educationDocument);
        }

        _pubSub.UpdateStatus(request.UserId);
        
        return Unit.Value;
    }
}