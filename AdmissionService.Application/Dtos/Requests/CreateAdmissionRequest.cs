using System.ComponentModel.DataAnnotations;
using AdmissionService.Application.Dtos.CustomValidationAtrribute;

namespace AdmissionService.Application.Dtos.Requests;

public class CreateAdmissionRequest
{
    [Required]
    public Guid ProgramId { get; set; }
    
    [Required]
    [Priority]
    public int Priority { get; set; }
    
}