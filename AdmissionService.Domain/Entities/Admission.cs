using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Models.Models.Enums;

namespace AdmissionService.Domain.Entities;

public class Admission
{
    [Key]
    public Guid AdmissionId { get; set; }
    
    [ForeignKey("Applicant")]
    public Guid ApplicantId { get; set; }

    [Required]
    public AdmissionStatus Status { get; set; }
    
    [Required]
    public int Priority { get; set; }
    
    [Required]
    public Guid ProgramId { get; set; }
    
    [Required]
    public Guid EducationLevelId { get; set; }
    
    public Applicant Applicant { get; set; }
    
    public Guid? ManagerId { get; set; }

    public bool IsDeleted { get; set; }
}