using System.ComponentModel.DataAnnotations;
using UserApi.Application.Dtos.CustomValidationAttributes;

namespace UserApi.Application.Dtos.Requests;

public class PasswordChangeDto
{
    [MinLength(6)]
    [MaxLength(30)]
    [Password]
    [Required(ErrorMessage = "Old password is required")]
    public string OldaPassword { get; set; }
      
    [MinLength(6)]
    [MaxLength(30)]
    [Password]
    [Required(ErrorMessage = "New password is required")]
    public string NewPassword { get; set; }
}