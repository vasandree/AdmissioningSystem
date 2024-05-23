using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Commands.Documents.UploadDocument;

public class UploadDocumentRequestHandler : IRequestHandler<UploadDocumentRequest, Unit>
{
    private readonly Helper _helper;
    private readonly IDocumentRepository<Passport> _passport;
    private readonly IDocumentRepository<EducationDocument> _educationDocument;

    public UploadDocumentRequestHandler(Helper helper, IDocumentRepository<Passport> passport,
        IDocumentRepository<EducationDocument> educationDocument)
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
                return await UploadPassport(request.Id, request.File);
            case DocumentType.EducationDocument:
                return await UploadEducationDocument(request.Id, request.File);
        }

        throw new BadRequest("Type of document was not chosen");
    }

    private async Task<Unit> UploadPassport(Guid id, IFormFile passport)
    {
        var existingPassport = (Passport)(await _passport.GetByUserId(id));

        if (existingPassport != null && existingPassport.File != null)
        {
            throw new BadRequest("You have already added file");
        }
        
        var file = await _helper.AddFile(passport);
        
        if (existingPassport == null)
        {
            var passportEntity = new Passport
            {
                Id = Guid.NewGuid(),
                DocumentType = DocumentType.Passport,
                UserId = id,
                File = file,
            };

            await _passport.CreateAsync(passportEntity);
        }
        else
        {
            existingPassport.File = file;

            await _passport.DeleteAsync(existingPassport);
        }
        

        return Unit.Value;
    }

    private async Task<Unit> UploadEducationDocument(Guid id, IFormFile passport)
    {
        var existingEducationDocument = (EducationDocument) await _educationDocument.GetByUserId(id);
        
        if (existingEducationDocument != null && existingEducationDocument.File != null )
        {
            throw new BadRequest("You have already uploaded file");
        }

        var file = await _helper.AddFile(passport);
        if (existingEducationDocument == null)
        {
            var educationDocument = new EducationDocument()
            {
                Id = new Guid(),
                DocumentType = DocumentType.EducationDocument,
                UserId = id,
                File = file,
            };

            await _educationDocument.CreateAsync(educationDocument);
        }
        else
        {
            existingEducationDocument.File = file;

            await _educationDocument.UpdateAsync(existingEducationDocument);
        }

        return Unit.Value;
    }
}