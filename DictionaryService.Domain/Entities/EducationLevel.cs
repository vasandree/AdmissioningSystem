using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class EducationLevel
{
    [Key]
    public int Id { get; set; }
    
    public string Name { get; set; }
}