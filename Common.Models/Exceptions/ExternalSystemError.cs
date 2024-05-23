using System.Net;

namespace Common.Models.Exceptions;

public class ExternalSystemError : Exception
{
    public ExternalSystemError(HttpStatusCode status) :
        base($"External system endpoint finished with status code: {status}")
    {
        
    }
}