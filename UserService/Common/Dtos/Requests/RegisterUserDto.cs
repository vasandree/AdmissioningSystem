using System.ComponentModel.DataAnnotations;
using UserService.Common.Enums;

namespace UserService.Dtos.CustomValidationAttributes;

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
    
    public Gender Gender { get; set; }
    
    [Phone]
    public string PhoneNumber { get; set; }

    public string Nationality { get; set; }
    
    [Date]
    public DateTime BirthDate { get; set; }

}
    
    
    
    