using System.ComponentModel.DataAnnotations;

namespace Common.Models.Dtos;

public class ProgramDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [Required]
    public string Code { get; set; }
    
    [Required]
    public string Language { get; set; }
    
    [Required]
    public string EducationForm { get; set; }
    
    [Required]
    public FacultyDto Faculty { get; set; }
    
    [Required]
    public EducationLevelDto EducationLevel { get; set; }
    
    [Required]
    public bool IsDeleted { get; set; }

}