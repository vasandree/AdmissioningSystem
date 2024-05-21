using System.ComponentModel.DataAnnotations;

namespace AdmissionService.Application.Dtos.Requests;

public class AdmissionRequestDto
{
    [Required] public Guid AdmissionId { get; set; }
}