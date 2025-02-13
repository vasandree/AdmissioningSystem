using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class Program : DictionaryEntity
{
    [Required] public Guid ExternalId { get; set; }
    
    [Required] public string Code { get; set; }

    [Required] public string Language { get; set; }

    [Required] public string EducationForm { get; set; }

    [Required] public Faculty Faculty { get; set; }

    [Required] public EducationLevel EducationLevel { get; set; }

    [Required] public DateTime CreateTime { get; set; }
}