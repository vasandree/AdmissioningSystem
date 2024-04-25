using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdmissionService.Domain.Enums;

namespace AdmissionService.Domain.Entities;

public class Admission
{
    [Key]
    public Guid AdmissionId { get; set; }
    
    [ForeignKey("Applicant")]
    public Guid ApplicantId { get; set; }

    public AdmissionStatus Status { get; set; }
    
    public int Priority { get; set; }
    
    // add programme
    
    public Applicant Applicant { get; set; }
}