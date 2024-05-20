using System.ComponentModel.DataAnnotations;

namespace DictionaryService.Domain.Entities;

public class NextEducationLevel
{
    [Key] public Guid Id { get; set; }

    [Required] public Guid EducationLevelId { get; set; }

    [Required] public Guid DocumentTypeId { get; set; }

    [Required] public int EducationLevelExternalId { get; set; }

    [Required] public Guid DocumentTypeExternalId { get; set; }
}