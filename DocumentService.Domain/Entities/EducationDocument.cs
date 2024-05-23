using Common.Models.Models.Dtos;

namespace DocumentService.Domain.Entities;

public class EducationDocument : Document
{
    public Guid? EducationDocumentTypeId { get; set; }
    
    public string? Name { get; set; }
    
}