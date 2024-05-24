using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Dtos.Requests;
using DocumentService.Application.Helpers;
using DocumentService.Application.RPC;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.AddEducationDocumentInfo;

public class AddEducationDocumentInfoCommandHandler : IRequestHandler<AddEducationDocumentInfoCommand, Unit>
{
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly RpcRequestSender _rpc;

    public AddEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> educationDocument,
        RpcRequestSender rpc)
    {
        _educationDocument = educationDocument;
        _rpc = rpc;
    }

    public async Task<Unit> Handle(AddEducationDocumentInfoCommand request, CancellationToken cancellationToken)
    {
        if (await _educationDocument.CheckExistence(request.UserId))
        {
            await ModifyEntity(request.UserId, request.EducationDocumentRequest);
        }
        else
        {
            await CreateNewEntity(request.UserId, request.EducationDocumentRequest);
        }

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