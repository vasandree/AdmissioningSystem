using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Dtos.CustomValidationAttributes;

public class PasswordAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is string password && password.Any(char.IsDigit))
        {
            return true;
        }
        ErrorMessage = "Password must contain at least one digit";
        return false;
    }
}