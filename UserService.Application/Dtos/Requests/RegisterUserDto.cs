using System.ComponentModel.DataAnnotations;
using Common.Models.Models.ValidationAttributes;
using UserApi.Domain.Enums;
using UserService.Application.Dtos.CustomValidationAttributes;

namespace UserService.Application.Dtos.Requests;

public class RegisterUserDto
{
    [MinLength(1)]
    [MaxLength(100)]
    [Required(ErrorMessage = "FullName is required")]
    public string FullName { get; set; }

    [MinLength(1)]
    [MaxLength(100)]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    
    [MinLength(1)]
    [MaxLength(30)]
    [Password]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    
    public Gender? Gender { get; set; }
    

    public string? Nationality { get; set; }
    
    [DateNotInFuture]
    public DateTime? BirthDate { get; set; }

}