using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Dtos.Requests;

public class ConfirmChangeDto
{
    [Required(ErrorMessage = "Email code is required")]
    public string EmailCode { get; set; }
}
