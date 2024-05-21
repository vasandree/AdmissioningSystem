using System.ComponentModel.DataAnnotations;
using AdmissionService.Application.Dtos.CustomValidationAttributes;
using Common.Models.Dtos;

namespace AdmissionService.Application.Dtos.Responses;

public class AdmissionDto
{
    [Required]
    public Guid AdmissionId { get; set; }
    
    [Required]
    [Priority]
    public int Priority { get; set; }
    
    [Required]
    public ProgramDto Program { get; set; }
}