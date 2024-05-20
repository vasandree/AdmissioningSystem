using System.ComponentModel.DataAnnotations;

namespace Common.Models.Dtos;


public class EducationDocumentTypeDto
{
    [Required]
    public Guid Id { get; set; }
    
    
    [Required]
    public string Name { get; set; }

    [Required]
    public EducationLevelDto EducationLevel { get; set; }
    
    public List<EducationLevelDto>? NextEducationLevels { get; set; }
    
    
}
