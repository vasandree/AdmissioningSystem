using System.ComponentModel.DataAnnotations;

namespace Common.Models.Models.Dtos;

public class ApplicantDto
{
    [Required] public Guid Id { get; set; }
    [Required] public string FullName { get; set; }
    [Required] public string Email { get; set; }
}