using System.ComponentModel.DataAnnotations;
using Common.Models.Models.Enums;
using Common.Models.Models.ValidationAttributes;

namespace Common.Models.Models.Dtos;

public class UserDto
{ 
    [Required]
    public Guid Id { get; set; }
    
    [MinLength(1)]
    [MaxLength(100)]
    [Required]
    public string FullName { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    
    public Gender? Gender { get; set; }
    
    public string? Nationality { get; set; }
    
    [DateNotInFuture]
    public DateTime? BirthDate { get; set; }
}