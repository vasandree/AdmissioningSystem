using System.ComponentModel.DataAnnotations;

namespace Common.Models.Models.Dtos;

public class ManagerDto
{
    [Required] public Guid Id { get; set; }
    [Required] public string FillName { get; set; }
    [Required] public string Email { get; set; }
    public Guid? FacultyId { get; set; }
}