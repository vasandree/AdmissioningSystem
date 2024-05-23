using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.EditEducationDocumentInfo;

public class EditEducationDocumentInfoCommandHandler : IRequestHandler<EditEducationDocumentInfoCommand, Unit>
{
    private readonly IDocumentRepository<EducationDocument> _eduactionDocument;
    private readonly RpcRequestSender _rpc;

    public EditEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> eduactionDocument, RpcRequestSender rpc)
    {
        _eduactionDocument = eduactionDocument;
        _rpc = rpc;
    }


    public async Task<Unit> Handle(EditEducationDocumentInfoCommand request, CancellationToken cancellationToken)
    {
        if (!await _eduactionDocument.CheckExistence(request.UserId))
        {
            throw new BadRequest("You haven't added education document yet");
        }

        var educationDocument = (EducationDocument)(await _eduactionDocument.GetByUserId(request.UserId))!;

        if (educationDocument.EducationDocumentTypeId == null || educationDocument.Name == null)
        {
            throw new BadRequest("You haven't added education document info yet");
        }
        
        var documentExists = await _rpc.CheckIfDocumentTypeExists(request.EducationDocumentRequest.EducationDocumentTypeId);
        if (documentExists.Exists == false)
            throw new BadRequest(
                $"DocumentType with {request.EducationDocumentRequest.EducationDocumentTypeId} does not exist");

        educationDocument.EducationDocumentTypeId = request.EducationDocumentRequest.EducationDocumentTypeId;
        educationDocument.Name = request.EducationDocumentRequest.Name;

        await _eduactionDocument.UpdateAsync(educationDocument);

        return Unit.Value;
    }
}