using System.ComponentModel.DataAnnotations;

namespace Common.Models.Models.ValidationAttributes;

public class DateNotInFutureAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
        {
            if (date <= DateTime.Now)
            {
                return true;
            }

            ErrorMessage = "Date can't be later than today";
        }
        else
        {
            ErrorMessage = "Invalid date format";
        }

        return false;
    }
}