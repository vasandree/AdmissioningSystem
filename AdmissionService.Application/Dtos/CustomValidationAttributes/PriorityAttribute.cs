using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Configuration;

namespace AdmissionService.Application.Dtos.CustomValidationAttributes;

public class PriorityAttribute : ValidationAttribute
{
    private readonly IConfiguration _config;

    public PriorityAttribute(IConfiguration config)
    {
        _config = config;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        int maxPriority = _config.GetValue<int>("MaxAdmissionsAmount");
        if (value is int priority)
        {
            if (priority < maxPriority)
            {
                return ValidationResult.Success;
            }
        }

        return new ValidationResult(ErrorMessage ?? $"Priority must be less than {maxPriority}");
    }
}