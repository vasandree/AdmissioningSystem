using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using BadRequest = Common.Exceptions.BadRequest;

namespace DocumentService.Application.Features.Commands.Documents.EditDocument;

public class EditDocumentCommandHandler : IRequestHandler<EditDocumentCommand, Unit>
{
    private readonly IDocumentRepository<Passport> _passport;
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly Helper _helper;

    public EditDocumentCommandHandler(IDocumentRepository<EducationDocument> educationDocument,
        IDocumentRepository<Passport> passport, Helper helper)
    {
        _educationDocument = educationDocument;
        _passport = passport;
        _helper = helper;
    }

    public async Task<Unit> Handle(EditDocumentCommand request, CancellationToken cancellationToken)
    {
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

        var fileEntity = await _helper.AddFile(file);

        var passportEntity = await _passport.GetByUserId(id);
        await _helper.UpdateFile(passportEntity!, fileEntity);

        return Unit.Value;
    }

    private async Task<Unit> EditEducationDocument(Guid id, IFormFile file)
    {
        if (!await _educationDocument.CheckExistence(id))
        {
            throw new BadRequest("You didn't upload the education document");
        }

        var fileEntity = await _helper.AddFile(file);

        var educationDocumentEntity = await _educationDocument.GetByUserId(id);
        await _helper.UpdateFile(educationDocumentEntity!, fileEntity);

        return Unit.Value;
    }
}