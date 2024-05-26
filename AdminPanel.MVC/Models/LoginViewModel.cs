using System.ComponentModel.DataAnnotations;

namespace AdminPanel.MVC.Models;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required]
    [Display(Name = "Password")]
    [MinLength(6)]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}