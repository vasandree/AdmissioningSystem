using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class Faculty
{
    [Key]
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    
    public DateTime CreateTime { get; set; }
}