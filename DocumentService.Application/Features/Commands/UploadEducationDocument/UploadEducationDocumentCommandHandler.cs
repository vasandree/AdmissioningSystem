using Common.Exceptions;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using DocumentService.Infrastructure;
using DocumentService.Persistence.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Commands.UploadEducationDocument;

public class UploadEducationDocumentCommandHandler : IRequestHandler<UploadEducationDocumentCommand, Unit>
{
    private readonly DocumentsDbContext _context;
    private readonly Helper _helper;

    public UploadEducationDocumentCommandHandler(DocumentsDbContext context, Helper helper)
    {
        _context = context;
        _helper = helper;
    }

    public async Task<Unit> Handle(UploadEducationDocumentCommand request, CancellationToken cancellationToken)
    {
        await UploadEducationDocument(request.UserId,request.File, request.DocumentTypeId, request.Name);

        return Unit.Value;
    }
    
    private async Task UploadEducationDocument(Guid id, IFormFile educationDocument, Guid documentTypeId, string name )
    {
        if (educationDocument == null || educationDocument.Length == 0)
        {
            throw new BadRequest("File is not provided or empty.");
        }

        //add documentTypeId checker
        
        var file = await _helper.AddFile(educationDocument, _context);

        var educationDocumentEntity = new EducationDocument
        {
            Id = new Guid(),
            DocumentType = DocumentType.Passport,
            UserId = id,
            File = file,
            Name = name,
            DocumentTypeId = documentTypeId
        };

        await _context.EducationDocuments.AddAsync(educationDocumentEntity);
        await _context.SaveChangesAsync();
    }
}