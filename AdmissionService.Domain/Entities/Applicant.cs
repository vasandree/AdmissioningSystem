using System.ComponentModel.DataAnnotations;
using Common.Models.Dtos;

namespace AdmissionService.Domain.Entities;

public class Applicant
{
    [Key]
    public Guid ApplicantId { get; set; }

    [Required]
    public EducationDocumentTypeDto EducationDocument { get; set; }
    
    public ICollection<Admission>? Admissions { get; set; }
}