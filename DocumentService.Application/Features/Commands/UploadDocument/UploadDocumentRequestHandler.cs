using Common.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Commands.UploadDocument;

public class UploadDocumentRequestHandler : IRequestHandler<UploadDocumentRequest, Unit>
{
    private readonly Helper _helper;
    private readonly IDocumentRepository<Passport> _passport;
    private readonly IDocumentRepository<EducationDocument> _educationDocument;

    public UploadDocumentRequestHandler(Helper helper, IDocumentRepository<Passport> passport, IDocumentRepository<EducationDocument> educationDocument)
    {
        _helper = helper;
        _passport = passport;
        _educationDocument = educationDocument;
    }

    public async Task<Unit> Handle(UploadDocumentRequest request, CancellationToken cancellationToken)
    {
        switch (request.DocumentType)
        {
            case DocumentType.Passport:
                await UploadPassport(request.Id, request.File);
                break;
            case DocumentType.EducationDocument:
                await UploadEducationDocument(request.Id, request.File);
                break;
        }
        
        return Unit.Value;
    }

    private async Task UploadPassport(Guid id, IFormFile passport)
    {
        if (await _passport.CheckExistence(id))
        {
            throw new BadRequest("You have already uploaded file");
        };
        
        var file = await _helper.AddFile(passport);

        var passportEntity = new Passport
        {
            Id = Guid.NewGuid(),
            DocumentType = DocumentType.Passport,
            UserId = id,
            File = file,
        };

        await _passport.CreateAsync(passportEntity);
    }

    private async Task UploadEducationDocument(Guid id, IFormFile passport)
    {
        
        if (await _educationDocument.CheckExistence(id))
        {
            throw new BadRequest("You have already uploaded file");
        };
        
        var file = await _helper.AddFile(passport);

        var educationDocument = new EducationDocument()
        {
            Id = new Guid(),
            DocumentType = DocumentType.EducationDocument,
            UserId = id,
            File = file,
        };

        await _educationDocument.CreateAsync(educationDocument);
    }
}