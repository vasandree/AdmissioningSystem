using System.ComponentModel.DataAnnotations;
using AdmissionService.Application.Dtos.CustomValidationAtrribute;

namespace AdmissionService.Application.Dtos.Requests;

public class ChangeAdmissionPriorityDto
{
    [Required]
    public Guid AdmissionId { get; set; }
    
    [Required]
    [Priority]
    public int Priority { get; set; }
}