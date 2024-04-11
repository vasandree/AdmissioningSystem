using System.ComponentModel.DataAnnotations;
using UserApi.Common.Dtos.CustomValidationAttributes;

namespace UserApi.Common.Dtos.Requests;

public class LoginUserDto
{
    [MinLength(1)]
    [MaxLength(100)]
    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; }
    
    [MinLength(6)]
    [MaxLength(30)]
    [Password]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
}
