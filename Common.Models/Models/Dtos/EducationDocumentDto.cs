using System.ComponentModel.DataAnnotations;

namespace Common.Models.Models.Dtos;

public class EducationDocumentDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public EducationDocumentTypeDto EducationDocumentType { get; set; }
}