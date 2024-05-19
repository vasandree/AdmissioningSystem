using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DictionaryService.Domain.Entities;

public class DocumentType : DictionaryEntity
{
    [Required] public Guid ExternalId { get; set; }

    [Required]
    [ForeignKey("EducationLevel")]
    public Guid EducationLevelId { get; set; }

    [Required] public EducationLevel EducationLevel { get; set; }

    public List<EducationLevel> NextEducationLevels { get; set; }

    [Required] public DateTime CreateTime { get; set; }
}