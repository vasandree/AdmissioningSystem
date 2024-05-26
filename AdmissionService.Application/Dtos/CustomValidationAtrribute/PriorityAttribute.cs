using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace AdmissionService.Application.Dtos.CustomValidationAtrribute;

public class PriorityAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        IConfiguration config = (IConfiguration)validationContext.GetService(typeof(IConfiguration))!;

        int maxPriority = ConfigurationBinder.GetValue<int>(config, "MaxAdmissionsAmount");
        if (value is int priority)
        {
            if (priority < maxPriority && priority >= 0)
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(ErrorMessage ??
                                    $"Priority must be less than {maxPriority} and greater than or equal to 0");
    }
}