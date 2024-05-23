using System.ComponentModel.DataAnnotations;

namespace Common.Models.Models.Dtos;

public class EducationLevelDto
{
    [Required] public Guid Id { get; set; }


    [Required] public string Name { get; set; }
}