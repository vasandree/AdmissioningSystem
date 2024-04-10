using System.ComponentModel.DataAnnotations;
using UserService.Dtos.CustomValidationAttributes;

namespace UserService.Common.Dtos.Requests;

public class ForgetPasswordDto
{
    [MinLength(6)]
    [MaxLength(30)]
    [Password]
    [Required(ErrorMessage = "New password is required")]
    public string NewPassword { get; set; }
}