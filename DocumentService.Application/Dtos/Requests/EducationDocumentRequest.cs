using System.ComponentModel.DataAnnotations;

namespace DocumentService.Application.Dtos.Requests;

public class EducationDocumentRequest
{
    [Required]
    public Guid EducationDocumentTypeId { get; set; }
    
    [Required]
    public string Name { get; set; }
}