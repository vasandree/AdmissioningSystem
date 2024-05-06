using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class Faculty
{
    [Key]
    public Guid Id { get; set; }
    
   [Required]
   public Guid ExternalId { get; set; }
    
   [Required] 
   public string Name { get; set; }
    
   [Required] 
   public DateTime CreateTime { get; set; }
}