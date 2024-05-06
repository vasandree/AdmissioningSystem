using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class EducationLevel
{
    
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public int ExternalId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
}