
using Common.Models.Consts.DocumentService;
using Common.Models.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using DocumentService.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.StaticFiles;

namespace DocumentService.Application.Helpers;

public class Helper
{
    private readonly IFileRepository _fileRepository;
    private readonly IDocumentRepository<Document> _documentRepository;

    public Helper(IFileRepository fileRepository, IDocumentRepository<Document> documentRepository)
    {
        _fileRepository = fileRepository;
        _documentRepository = documentRepository;
    }

    public async Task<DbFile> AddFile(IFormFile file, DocumentType documentType)
    {
        using (var memoryStream = new MemoryStream())
        {
            if (file == null || file.Length == 0)
            {
                throw new BadRequest("File is not provided or empty.");
            }

            
            string fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (file.Length > FileConsts.FileSizeLimit)
            {
                throw new BadRequest("File size exceeds the allowed limit.");
            }

            
            if (!FileConsts.AllowedExtensions.Contains(fileExtension))
            {
                throw new BadRequest("Invalid file extension.");
            }
            
            await file.CopyToAsync(memoryStream);
            var id = Guid.NewGuid();
            var fileEntity = new DbFile()
            {
                Id = id,
                FileName = $"{documentType.ToString()}_{id}{fileExtension}",
                FileContent = memoryStream.ToArray()
            };

            await _fileRepository.CreateAsync(fileEntity);
            
            return fileEntity;
        }
    }

    public async Task UpdateFile(Document document, DbFile newFile)
    {
        if (document.File == null) throw new BadRequest("You haven't uploaded file yet");
        var oldFile = await _fileRepository.GetById(document.File!.Id);
        document.File = newFile;
        await _documentRepository.UpdateAsync(document);
        if (oldFile != null) await _fileRepository.DeleteAsync(oldFile);
    }

    public async Task DeleteFile(Guid id)
    {
        var fileEntity = await _fileRepository.GetById(id);
        if (fileEntity != null) await _fileRepository.DeleteAsync(fileEntity);
    }

    public (byte[], string, string) ConvertToFile(DbFile file)
    {
        var provider = new FileExtensionContentTypeProvider();
        if (!provider.TryGetContentType(file.FileName, out var _contentType))
        {
            _contentType = "application/octet-stream";
        }

        return (file.FileContent, _contentType, file.FileName);

    }

}