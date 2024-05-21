using Common.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using MediatR;

namespace DocumentService.Application.Features.Commands.Documents.DeleteDocument;

public class DeleteDocumentCommandHandler : IRequestHandler<DeleteDocumentCommand, Unit>
{
    private readonly IDocumentRepository<Passport> _passport;
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly Helper _helper;

    public DeleteDocumentCommandHandler(IDocumentRepository<Passport> passport,
        IDocumentRepository<EducationDocument> educationDocument, Helper helper)
    {
        _passport = passport;
        _educationDocument = educationDocument;
        _helper = helper;
    }

    public async Task<Unit> Handle(DeleteDocumentCommand request, CancellationToken cancellationToken)
    {
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

        if (passportEntity.IssueDate != null || passportEntity.IssuedBy != null ||
            passportEntity.SeriesAndNumber != null || passportEntity.DateOfBirth != null)
        {
            passportEntity.File = null;
            await _passport.UpdateAsync(passportEntity);
        }
        else
        {
            await _passport.DeleteAsync(passportEntity);
        }

        await _helper.DeleteFile(passportEntity!.File.Id);

        return Unit.Value;
    }

    private async Task<Unit> DeleteEducationDocument(Guid id)
    {
        if (!await _educationDocument.CheckExistence(id))
        {
            throw new BadRequest("There is no education document to delete");
        }

        var educationDocumentEntity = (EducationDocument)(await _educationDocument.GetByUserId(id))!;

        if (educationDocumentEntity.EducationDocumentType != null || educationDocumentEntity.Name != null)
        {
            educationDocumentEntity.File = null;
        }
        else
        {
            await _educationDocument.DeleteAsync(educationDocumentEntity);

        }
        
        await _helper.DeleteFile(educationDocumentEntity!.File.Id);

        return Unit.Value;
    }
}