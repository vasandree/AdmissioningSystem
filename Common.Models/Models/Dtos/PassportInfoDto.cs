using System.ComponentModel.DataAnnotations;
using Common.Models.Models.ValidationAttributes;

namespace Common.Models.Models.Dtos;

public class PassportInfoDto
{
    [Required]
    public Guid Id { get; set; }
    
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