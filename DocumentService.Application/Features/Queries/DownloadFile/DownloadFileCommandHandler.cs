using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Application.Helpers;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using MediatR;

namespace DocumentService.Application.Features.Queries.DownloadFile;

public class DownloadFileCommandHandler : IRequestHandler<DownloadFileCommand, (byte[], string, string)>
{
    private readonly IDocumentRepository<Passport> _passport;
    private readonly IDocumentRepository<EducationDocument> _educationDocument;
    private readonly Helper _helper;

    public DownloadFileCommandHandler(IDocumentRepository<Passport> passport, IDocumentRepository<EducationDocument> educationDocument, Helper helper)
    {
        _passport = passport;
        _educationDocument = educationDocument;
        _helper = helper;
    }

    public async Task<(byte[], string, string)> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
    {
        switch (request.DocumentType)
        {
            case DocumentType.Passport:
                return await DownloadPassport(request.UserId);
            case DocumentType.EducationDocument:
                return await DownloadEducationDocument(request.UserId);
        }

        throw new BadRequest("Type of document was not chosen");
    }

    private async Task<(byte[], string, string)> DownloadPassport(Guid userId)
    {
        var passportEntity = await _passport.GetByUserId(userId);
        if (passportEntity != null && passportEntity.File != null) return  _helper.ConvertToFile(passportEntity!.File);
        throw new BadRequest("No passport was added to download");
    }
    
    private async Task<(byte[], string, string)> DownloadEducationDocument(Guid userId)
    {
        var educationDocumentEntity = await _educationDocument.GetByUserId(userId);
        if (educationDocumentEntity != null && educationDocumentEntity.File != null) return _helper.ConvertToFile(educationDocumentEntity!.File);
        throw new BadRequest("No education document was added to download");
    }
}