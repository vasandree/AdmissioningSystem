using System.ComponentModel.DataAnnotations;
using Common.Models.ValidationAttributes;

namespace DocumentService.Application.Dtos.Requests;

public class PassportInfoRequest
{
    [Required]
    public string SeriesAndNumber { get; set; }
    
    [Required]
    public DateTime BirthDate { get; set; }
    
    [Required]
    [Date]
    public DateTime IssueDate { get; set; }
    
    [Required]
    [Date]
    public string IssuedBy { get; set; }
}