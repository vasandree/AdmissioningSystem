using System.ComponentModel.DataAnnotations;
using UserService.Application.Dtos.CustomValidationAttributes;

namespace UserService.Application.Dtos.Requests;

public class ForgetPasswordDto
{
        [MinLength(6)]
        [MaxLength(30)]
        [Password]
        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }
        
        [Required(ErrorMessage = "Confirm code is required")]
        public string ConfirmCode { get; set; }
}