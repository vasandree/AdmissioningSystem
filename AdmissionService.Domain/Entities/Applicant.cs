using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Dtos;

namespace AdmissionService.Domain.Entities;

public class Applicant
{
    [Key]
    public Guid ApplicantId { get; set; }

    [Required]
    public Guid EducationDocumentId { get; set; }
    
    public ICollection<Admission>? Admissions { get; set; }
}