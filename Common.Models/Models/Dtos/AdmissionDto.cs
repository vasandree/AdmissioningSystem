using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Enums;

namespace Common.Models.Models.Dtos;

public class AdmissionDto
{
    [Required] public Guid Id { get; set; }
    [Required] public ApplicantDto Applicant { get; set; }
    
    [Required] public int Priority { get; set; }
    
    [Required] public AdmissionStatus Status { get; set; }
    
    [Required] public ProgramDto Program { get; set; }
    
    public ManagerDto? Manager { get; set; }
}
