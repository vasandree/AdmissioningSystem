using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class EducationLevel : DictionaryEntity
{
    [Required] public int ExternalId { get; set; }
    
}