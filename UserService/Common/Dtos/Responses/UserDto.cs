using System.ComponentModel.DataAnnotations;
using UserService.Common.Enums;
using UserService.Dtos.CustomValidationAttributes;

namespace UserService.Common.Dtos.Responses;

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
    
    [Required]
    public Gender Gender { get; set; }
    
    [Phone]
    [Required]
    public string PhoneNumber { get; set; }

    [Required]
    public string Nationality { get; set; }
    
    [Date]
    [Required]
    public DateTime BirthDate { get; set; }
}