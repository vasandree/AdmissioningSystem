namespace Common.Exceptions;

public class ExternalSystemException : Exception
{
    public ExternalSystemException(Exception exception) : 
        base($"Failed to connect to external system ", exception)
    {
    }
}