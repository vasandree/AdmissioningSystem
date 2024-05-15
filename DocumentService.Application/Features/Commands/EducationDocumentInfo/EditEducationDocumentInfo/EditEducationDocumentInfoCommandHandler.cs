using Common.Exceptions;
using Common.Models.Dtos;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.EditEducationDocumentInfo;

public class EditEducationDocumentInfoCommandHandler : IRequestHandler<EditEducationDocumentInfoCommand, Unit>
{
    private readonly IDocumentRepository<EducationDocument> _eduactionDocument;

    public EditEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> eduactionDocument)
    {
        _eduactionDocument = eduactionDocument;
    }


    public async Task<Unit> Handle(EditEducationDocumentInfoCommand request, CancellationToken cancellationToken)
    {
        if (!await _eduactionDocument.CheckExistence(request.UserId))
        {
            throw new BadRequest("You haven't added education document yet");
        }

        var educationDocument = (EducationDocument)(await _eduactionDocument.GetByUserId(request.UserId))!;

        if (educationDocument.EducationDocumentType == null || educationDocument.Name == null)
        {
            throw new BadRequest("You haven't added education document info yet");
        }
        
        //TODO: check if educationDocumentRequest.EducationDocumentTypeId exists

        EducationDocumentTypeDto? educationDocumentType = null;

        educationDocument.EducationDocumentType = educationDocumentType;
        educationDocument.Name = request.EducationDocumentRequest.Name;

        await _eduactionDocument.UpdateAsync(educationDocument);

        return Unit.Value;
    }
}