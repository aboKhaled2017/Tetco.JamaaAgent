using Domain.Enums;

namespace Domain.Common.Patterns;

public class Result : Result<object>
{
    public Result() // only for serializing
    {

    }
    internal Result(bool succeeded, IDictionary<string, string[]> errors, string message = null, AgentErrorType errorType = AgentErrorType.Technical) : base(succeeded, errors, message,errorType)
    {
    }

    public static Result Success(string message = null,AgentErrorType errorType = AgentErrorType.Technical)
    {
        return new Result(true, null)
        {
            Message = message,
            ErrorType = errorType.ToString()
        };
    }
    public static Result Failure(string errorCode, string errorMessage,AgentErrorType errorType= AgentErrorType.Technical)
    {
        return new Result(false, new Dictionary<string, string[]>() )
        {
            Code = errorCode,
            Message = errorMessage,
            ErrorType = errorType.ToString()
        };
    }
    public static Result Failure(string errorCode, IDictionary<string, string[]> errors, string message = null, AgentErrorType errorType = AgentErrorType.Technical)
    {
        return new Result(false, errors,message,errorType)
        {
            Code = errorCode
        };
    }
}
public class Result<TData>
{
    public Result() // only for serializing
    {

    }
    internal Result(bool succeeded, IDictionary<string, string[]> errors, string message = null, AgentErrorType errorType=AgentErrorType.Technical)
    {
        Succeeded = succeeded;
        Errors = errors;
        Message = message;
        ErrorType = errorType.ToString();
    }

    public bool Succeeded
    {
        get; init;
    }
    public IDictionary<string, string[]> Errors
    {
        get; init;
    }
    public string Message { get; set; }
    public string Code { get; set; }
    public string ErrorType { get; set; }

    public static Result<TData> Success(string message = null,AgentErrorType errorType= AgentErrorType.Technical)
    {
        return new Result<TData>(true,null)
        {
            Message = message,
            ErrorType = errorType.ToString()
        };
    }
    public static Result<TData> Failure(string errorCode, string errorMessage, AgentErrorType errorType=AgentErrorType.Technical)
    {
        return new Result<TData>(false,new Dictionary<string, string[]>())
        {
            Code = errorCode,
            Message = errorMessage,
            ErrorType = errorType.ToString()
        };
    }
    public static Result<TData> Failure(string errorCode, IDictionary<string, string[]> errors, AgentErrorType errorType = AgentErrorType.Technical)
    {
        return new Result<TData>(false, errors)
        {
            Code = errorCode
        };
    }
    public TData Data { get; set; }
    public Result<TData> WithData(TData data)
    {
        Data = data;
        return this;
    }
}
