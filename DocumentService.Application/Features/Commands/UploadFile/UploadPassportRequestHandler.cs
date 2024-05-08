using Common.Exceptions;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using DocumentService.Infrastructure;
using DocumentService.Persistence.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Features.Commands.UploadFile;

public class UploadPassportRequestHandler : IRequestHandler<UploadPassportRequest, Unit>
{
    private readonly DocumentsDbContext _context;
    private readonly Helper _helper;

    public UploadPassportRequestHandler(DocumentsDbContext context, Helper helper)
    {
        _context = context;
        _helper = helper;
    }

    public async Task<Unit> Handle(UploadPassportRequest request, CancellationToken cancellationToken)
    {
        await UploadPassport(request.Id, request.File);

        return Unit.Value;
    }

    private async Task UploadPassport(Guid id, IFormFile passport)
    {
        if (passport == null || passport.Length == 0)
        {
            throw new BadRequest("File is not provided or empty.");
        }

        var file = await _helper.AddFile(passport, _context);

        var passportEntity = new Passport
        {
            Id = new Guid(),
            DocumentType = DocumentType.Passport,
            UserId = id,
            File = file,
        };

        await _context.Passports.AddAsync(passportEntity);
        await _context.SaveChangesAsync();
    }

  
}