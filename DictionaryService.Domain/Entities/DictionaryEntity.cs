using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public abstract class DictionaryEntity
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }

    public bool IsDeleted { get; set; }
}