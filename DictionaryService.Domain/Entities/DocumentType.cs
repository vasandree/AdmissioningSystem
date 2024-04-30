using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DictionaryService.Domain.Entities;

public class DocumentType
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    [ForeignKey("EducationLevel")]
    public int EducationLevelId { get; set; }
    
    public EducationLevel EducationLevel { get; set; }
    
    public ICollection<EducationLevel> NextEducationLevels { get; set; }
    
    public DateTime CreateTime { get; set; }
}