namespace Domain.Common.Patterns;

public class Result : Result<object>
{
    public Result() // only for serializing
    {

    }
    internal Result(bool succeeded, IDictionary<string, string[]> errors, string message = null) : base(succeeded, errors, message)
    {
    }

    public static Result Success(string message = null)
    {
        return new Result(true, null)
        {
            Message = message
        };
    }
    public static Result Failure(string errorCode, string errorMessage)
    {
        return new Result(false, new Dictionary<string, string[]>())
        {
            Code = errorCode,
            Message = errorMessage
        };
    }
    public static Result Failure(string errorCode, IDictionary<string, string[]> errors, string message = null)
    {
        return new Result(false, errors,message)
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
    internal Result(bool succeeded, IDictionary<string, string[]> errors, string message = null)
    {
        Succeeded = succeeded;
        Errors = errors;
        Message = message;
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

    public static Result<TData> Success(string message = null)
    {
        return new Result<TData>(true,null)
        {
            Message = message
        };
    }
    public static Result<TData> Failure(string errorCode, string errorMessage)
    {
        return new Result<TData>(false,new Dictionary<string, string[]>())
        {
            Code = errorCode,
            Message = errorMessage
        };
    }
    public static Result<TData> Failure(string errorCode, IDictionary<string, string[]> errors)
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
