using System.ComponentModel.DataAnnotations;

namespace Common.Models.Models.ValidationAttributes;

public class DateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date && date < DateTime.Now || value is null)
        {
            return true;
        }

        ErrorMessage = "Date can't be later than today";
        return false;
    }
}