using System.ComponentModel.DataAnnotations;

namespace UserApi.Common.Dtos.Requests;

public class ConfirmChangeDto
{
    [Required(ErrorMessage = "Email code is required")]
    public string EmailCode { get; set; }
}