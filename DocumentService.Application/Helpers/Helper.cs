using Common.Consts.DocumentService;
using Common.Exceptions;
using DocumentService.Application.Contracts.Persistence;
using DocumentService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using File = DocumentService.Domain.Entities.File;

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

    public async Task<File> AddFile(IFormFile file)
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
            var fileEntity = new File()
            {
                Id = Guid.NewGuid(),
                FileContent = memoryStream.ToArray()
            };

            await _fileRepository.CreateAsync(fileEntity);
            
            return fileEntity;
        }
    }

    public async Task UpdateFile(Document document, File newFile)
    {
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
}