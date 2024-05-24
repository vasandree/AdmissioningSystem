namespace Common.ServiceBus.RabbitMqMessages;

public abstract class BaseRpcHandler
{
    public abstract void CreateRequestListeners();
    
    
    protected T HandleException<T>(T response) where T : BaseResponse
    {
        if (response.ExceptionToThrow != null) throw response.ExceptionToThrow;
        return response;
    }
}