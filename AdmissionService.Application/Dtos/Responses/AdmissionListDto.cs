using System.ComponentModel.DataAnnotations;
using AdmissionService.Application.Dtos.CustomValidationAtrribute;
using Common.Models.Models.Enums;

namespace AdmissionService.Application.Dtos.Responses;

public class AdmissionListDto
{
    [Required]
    public Guid AdmissionId { get; set; }
    
    [Required]
    [Priority]
    public int Priority { get; set; }
    
    [Required]
    public string ProgramName { get; set; }
    
    [Required]
    public AdmissionStatus Status { get; set; }
}