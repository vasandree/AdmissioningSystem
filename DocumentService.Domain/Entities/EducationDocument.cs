using Common.Models.Dtos;

namespace DocumentService.Domain.Entities;

public class EducationDocument : Document
{
    public EducationDocumentTypeDto? EducationDocumentType { get; set; }
    
    public string? Name { get; set; }
}