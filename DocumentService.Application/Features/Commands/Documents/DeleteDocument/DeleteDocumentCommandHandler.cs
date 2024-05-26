using Common.Models.Exceptions;
using Common.Models.Models.Enums;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Application.ServiceBus.PubSub.Sender;
using DocumentService.Application.ServiceBus.RPC.RpcSender;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.Documents.DeleteDocument;

public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Unit>
{
    private readonly IDocumentRepository<Passport> _passport;
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly PubSubSender _pubSub;
    private readonly RpcRequestSender _rpc;
    private readonly Helper _helper;

    public DeleteDocumentCommandHandler(IDocumentRepository<Passport> passport,
        IDocumentRepository<EducationDocument> educationDocument, Helper helper, RpcRequestSender rpc, PubSubSender pubSub)
    {
        _passport = passport;
        _educationDocument = educationDocument;
        _helper = helper;
        _rpc = rpc;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
        if (await _rpc.CheckStatusClosed(request.Id))
            throw new BadRequest("You cannot edit your profile, because one of your admissions is closed");

        
        switch (request.DocumentType)
        {
            case DocumentType.Passport:
                return await DeletePassport(request.Id);
            case DocumentType.EducationDocument:
                return await DeleteEducationDocument(request.Id);
        }

        throw new BadRequest("Type of document was not chosen");
    }

    private async Task<Unit> DeletePassport(Guid id)
    {
        if (!await _passport.CheckExistence(id))
        {
            throw new BadRequest("There is no passport to delete");
        }

        var passportEntity = (Passport)(await _passport.GetByUserId(id))!;

        if (passportEntity.File == null)
            throw new BadRequest("There is no passport to delete");
        
        if (passportEntity.IssueDate != null || passportEntity.IssuedBy != null ||
            passportEntity.SeriesAndNumber != null || passportEntity.DateOfBirth != null)
        {
            await _helper.DeleteFile(passportEntity!.File.Id);
        }
        else
        {
            await _helper.DeleteFile(passportEntity!.File.Id);
            await _passport.DeleteAsync(passportEntity);
        }


        _pubSub.UpdateStatus(id);
        
        return Unit.Value;
    }

    private async Task<Unit> DeleteEducationDocument(Guid id)
    {
        if (!await _educationDocument.CheckExistence(id))
        {
            throw new BadRequest("There is no education document to delete");
        }

        var educationDocumentEntity = (EducationDocument)(await _educationDocument.GetByUserId(id))!;

        if (educationDocumentEntity.File == null)
            throw new BadRequest("There is no education document to delete");

        if (educationDocumentEntity.EducationDocumentTypeId != null || educationDocumentEntity.Name != null)
        {
            await _helper.DeleteFile(educationDocumentEntity!.File.Id);
        }
        else
        {
            await _helper.DeleteFile(educationDocumentEntity!.File.Id);
            await _educationDocument.DeleteAsync(educationDocumentEntity);
        }

        _pubSub.UpdateStatus(id);
        
        return Unit.Value;
    }
}