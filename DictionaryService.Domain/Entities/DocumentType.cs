using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class DocumentType
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public EducationLevel EducationLevel { get; set; }
    
    public List<EducationLevel> NextEducationLevels { get; set; }
    
    public DateTime CreateTime { get; set; }
}