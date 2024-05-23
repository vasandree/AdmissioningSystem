namespace Common.Models.Exceptions;

public class SmtpServerException : Exception
{
    public SmtpServerException(Exception exception, string email, string action) 
        : base($"Failed to send the {action} email to {email}", exception)
    {
    }
}