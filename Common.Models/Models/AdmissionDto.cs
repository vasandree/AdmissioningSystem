using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Dtos;

namespace Common.Models.Models;

public class AdmissionDto
{
    [Required]
    public Guid AdmissionId { get; set; }
    
    [Required]
    public int Priority { get; set; }
    
    [Required]
    public ProgramDto Program { get; set; }
}