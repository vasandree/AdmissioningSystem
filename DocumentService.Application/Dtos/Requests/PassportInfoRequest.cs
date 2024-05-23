using System.ComponentModel.DataAnnotations;
using Common.Models.Models.ValidationAttributes;

namespace DocumentService.Application.Dtos.Requests;

public class PassportInfoRequest
{
    [Required]
    public string SeriesAndNumber { get; set; }
    
    [Required]
    [DateNotInFuture]
    public DateTime BirthDate { get; set; }
    
    [Required]
    [DateNotInFuture]
    public DateTime IssueDate { get; set; }
    
    [Required]
    public string IssuedBy { get; set; }
}