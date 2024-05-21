using System.ComponentModel.DataAnnotations;

namespace DocumentService.Domain.Entities;

public class Passport : Document
{
   public string? SeriesAndNumber { get; set; }
   
   public DateTime? DateOfBirth { get; set; }
   
   public DateTime? IssueDate { get; set; }
   
   public string? IssuedBy { get; set; }
}