using System.ComponentModel.DataAnnotations;

namespace DocumentService.Domain.Entities;

public class EducationDocument : Document
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public Guid DocumentTypeId { get; set; }
}