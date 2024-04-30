using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DictionaryService.Domain.Entities;

public class Program
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public string Code { get; set; }
    
    public string Language { get; set; }
    
    public string EducationForm { get; set; }
    
    [ForeignKey("Faculty")]
    public Guid FacultyId { get; set; }
    
    [ForeignKey("EducationLevel")]
    public int EducationLevelId { get; set; }
    
    public Faculty Faculty { get; set; }
    
    public EducationLevel EducationLevel { get; set; }
    
    public DateTime CreateTime { get; set; }
}