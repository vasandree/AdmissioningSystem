using System.ComponentModel.DataAnnotations;

namespace UserApi.Application.Dtos.Requests;

public class ChangeEmailDto
{
    [EmailAddress]
    [Required(ErrorMessage = "Old email is required")]
    public string OldEmail { get; set; }
    
    [EmailAddress]
    [Required(ErrorMessage = "New email is required")]
    public string NewEmail { get; set; }
    
}