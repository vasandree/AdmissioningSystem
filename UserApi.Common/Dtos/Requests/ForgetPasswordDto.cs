using System.ComponentModel.DataAnnotations;
using UserApi.Common.Dtos.CustomValidationAttributes;

namespace UserApi.Common.Dtos.Requests;

public class ForgetPasswordDto
{
        [MinLength(6)]
        [MaxLength(30)]
        [Password]
        [Required(ErrorMessage = "New password is required")]
        public string NewPassword { get; set; }
}