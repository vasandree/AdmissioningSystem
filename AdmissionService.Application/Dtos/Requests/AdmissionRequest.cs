using System.ComponentModel.DataAnnotations;

namespace AdmissionService.Application.Dtos.Requests;

public class AdmissionRequest
{
    [Required]
    public Guid ProgramId { get; set; }
    
    [Required]
    public int Priority { get; set; }
    
}