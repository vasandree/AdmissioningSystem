using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class DocumentType
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid ExternalId { get; set; }
    
    [Required]
    public string Name { get; set; }

    [Required]
    public Guid EducationLevelId { get; set; }
    public EducationLevel EducationLevel { get; set; }
    
    public List<EducationLevel>? NextEducationLevels { get; set; }
    
    [Required]
    public DateTime CreateTime { get; set; }
}