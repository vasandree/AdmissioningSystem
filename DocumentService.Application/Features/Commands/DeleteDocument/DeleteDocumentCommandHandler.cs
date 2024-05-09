using Common.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using MediatR;

namespace DocumentService.Application.Features.Commands.DeleteDocument;

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

        var passportEntity = await _passport.GetByUserId(id);
        await _passport.DeleteAsync(passportEntity! as Passport);
        await _helper.DeleteFile(passportEntity!.File.Id);

        return Unit.Value;
    }

    private async Task<Unit> DeleteEducationDocument(Guid id)
    {
        if (!await _educationDocument.CheckExistence(id))
        {
            throw new BadRequest("There is no education document to delete");
        }

        var educationDocumentEntity = await _educationDocument.GetByUserId(id);
        await _educationDocument.DeleteAsync(educationDocumentEntity! as EducationDocument);
        await _helper.DeleteFile(educationDocumentEntity!.File.Id);

        return Unit.Value;
    }
}