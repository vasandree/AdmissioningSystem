using System.ComponentModel.DataAnnotations;
using UserApi.Domain.Enums;
using UserService.Application.Dtos.CustomValidationAttributes;

namespace UserService.Application.Dtos.Responses;

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
    
    [Date]
    public DateTime? BirthDate { get; set; }
}