using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class Faculty : DictionaryEntity
{
   [Required]
   public Guid ExternalId { get; set; }
   
   [Required] 
   public DateTime CreateTime { get; set; }

}