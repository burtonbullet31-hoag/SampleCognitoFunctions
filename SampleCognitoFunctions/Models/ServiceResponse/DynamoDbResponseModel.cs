using SampleCognitoFunctions.Enums;

namespace SampleCognitoFunctions.Models.ServiceResponse;

public class DynamoDbResponseModel<TData>(
    TData? data,
    ServiceResponseStatus status,
    DynamoDbResponseModel<TData>.ErrorResponse? error)
{
    public class ErrorResponse(Exception innerException)
    {
        public string Message => InnerException.Message;

        public Exception InnerException { get; private set; } = innerException;
    }
    
    public TData? Data { get; private set; } = data;
    public ServiceResponseStatus Status { get; private set; } = status;
    public ErrorResponse? Error { get; private set; } = error;
}