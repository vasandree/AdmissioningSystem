using Common.Models.Dtos;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Dtos.Requests;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using MediatR;

namespace DocumentService.Application.Features.Commands.EducationDocumentInfo.AddEducationDocumentInfo;

public class AddEducationDocumentInfoCommandHandler : IRequestHandler<AddEducationDocumentInfoCommand, Unit>
{
    private readonly IDocumentRepository<EducationDocument> _educationDocument;

    public AddEducationDocumentInfoCommandHandler(IDocumentRepository<EducationDocument> educationDocument)
    {
        _educationDocument = educationDocument;
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
        //TODO: check if educationDocumentRequest.EducationDocumentTypeId exists
        EducationDocumentTypeDto? educationDocumentType = null;
        
        await _educationDocument.CreateAsync(new EducationDocument
        {
            Id = Guid.NewGuid(),
            DocumentType = DocumentType.Passport,
            UserId = userId,
            File = null,
            EducationDocumentType = educationDocumentType,
            Name = educationDocumentRequest.Name
        });
    }

    private async Task ModifyEntity(Guid userId, EducationDocumentRequest educationDocumentRequest)
    {
        var educationDocument = (EducationDocument)(await _educationDocument.GetByUserId(userId))!;
        
        //TODO: check if educationDocumentRequest.EducationDocumentTypeId exists
        EducationDocumentTypeDto? educationDocumentType = null;

        educationDocument.EducationDocumentType = educationDocumentType;
        educationDocument.Name = educationDocumentRequest.Name;
    }
}
