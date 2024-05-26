using System.ComponentModel.DataAnnotations;

namespace AdminPanel.MVC.Models;

public class LoginViewModel
{
    [Microsoft.Build.Framework.Required]
    [EmailAddress]
    public string Email { get; set; }

    [Microsoft.Build.Framework.Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public bool RememberMe { get; set; }
}