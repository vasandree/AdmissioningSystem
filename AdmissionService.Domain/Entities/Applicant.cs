using System.ComponentModel.DataAnnotations;

namespace AdmissionService.Domain.Entities;

public class Applicant
{
    [Key]
    public Guid ApplicantId { get; set; }

    private ICollection<Admission> Admissions { get; set; }
}