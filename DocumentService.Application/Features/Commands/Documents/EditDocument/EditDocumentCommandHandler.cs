using Common.Models.Exceptions;
using Common.Models.Models.Enums;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Application.ServiceBus.PubSub.Sender;
using DocumentService.Application.ServiceBus.RPC.RpcSender;
using DocumentService.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Commands.Documents.EditDocument;

public class EditDocumentCommandHandler : IRequestHandler<EditDocumentCommand, Unit>
{
    private readonly IDocumentRepository<Passport> _passport;
    private readonly RpcRequestSender _rpc;
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly PubSubSender _pubSub;
    private readonly Helper _helper;

    public EditDocumentCommandHandler(IDocumentRepository<EducationDocument> educationDocument,
        IDocumentRepository<Passport> passport, Helper helper, RpcRequestSender rpc, PubSubSender pubSub)
    {
        _educationDocument = educationDocument;
        _passport = passport;
        _helper = helper;
        _rpc = rpc;
        _pubSub = pubSub;
    }

    public async Task<Unit> Handle(EditDocumentCommand request, CancellationToken cancellationToken)
    {
        if (await _rpc.CheckStatusClosed(request.Id))
            throw new BadRequest("You cannot edit your profile, because one of your admissions is closed");


        
        switch (request.DocumentType)
        {
            case DocumentType.Passport:
                return await EditPassport(request.Id, request.File);

            case DocumentType.EducationDocument:
                return await EditEducationDocument(request.Id, request.File);
        }

        
        
        throw new BadRequest("Type of document was not chosen");
    }

    private async Task<Unit> EditPassport(Guid id, IFormFile file)
    {
        if (!await _passport.CheckExistence(id))
        {
            throw new BadRequest("You didn't upload the passport");
        }

        var fileEntity = await _helper.AddFile(file, DocumentType.Passport);

        var passportEntity = await _passport.GetByUserId(id);
        await _helper.UpdateFile(passportEntity!, fileEntity);

        _pubSub.UpdateStatus(id);
        
        return Unit.Value;
    }

    private async Task<Unit> EditEducationDocument(Guid id, IFormFile file)
    {
        if (!await _educationDocument.CheckExistence(id))
        {
            throw new BadRequest("You didn't upload the education document");
        }

        var fileEntity = await _helper.AddFile(file, DocumentType.EducationDocument);

        var educationDocumentEntity = await _educationDocument.GetByUserId(id);
        await _helper.UpdateFile(educationDocumentEntity!, fileEntity);

        _pubSub.UpdateStatus(id);
        
        return Unit.Value;
    }
}