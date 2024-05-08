using Microsoft.AspNetCore.Http;

namespace DocumentService.Application.Dtos;

public class UploadEducationDocumentRequest
{
    public string Name { get; set; }
    public Guid DocumentTypeId { get; set; }
    
    public IFormFile File { get; set; }
}