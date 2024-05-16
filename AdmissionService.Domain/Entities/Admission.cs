using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AdmissionService.Domain.Enums;
using Common.Models.Dtos;

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
    public ProgramDto Program { get; set; }
    
    public Applicant Applicant { get; set; }
}