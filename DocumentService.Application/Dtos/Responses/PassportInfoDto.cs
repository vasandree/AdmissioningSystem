using System.ComponentModel.DataAnnotations;
using Common.Models.Models.ValidationAttributes;

namespace DocumentService.Application.Dtos.Responses;

public class PassportInfoDto
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    public string SeriesAndNumber { get; set; }
    
    [Required]
    [Date]
    public DateTime BirthDate { get; set; }
    
    [Required]
    [Date]
    public DateTime IssueDate { get; set; }
    
    [Required]
    public string IssuedBy { get; set; }
}