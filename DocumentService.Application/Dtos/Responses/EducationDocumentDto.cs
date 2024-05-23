using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Dtos;

namespace DocumentService.Application.Dtos.Responses;

public class EducationDocumentDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public EducationDocumentTypeDto EducationDocumentType { get; set; }
}