using Domain.Common.Patterns;

namespace Domain.Common.Exceptions;

public abstract class BaseJamaaAgentException: Exception
{
    public BaseJamaaAgentException(string code,string errorMessage):base(errorMessage)
    {
        Code = code;
    }
    public IDictionary<string, string[]> Errors { get; set; }
    public string Code { get; set; }
    
    public TException WithError<TException>(string key, IEnumerable<string> errors)
        where TException : BaseJamaaAgentException

    {
        if(!Errors.ContainsKey(key))
          Errors.Add(key, errors.ToArray());
      
        return this as TException;
    }

    public Result ToResultError()
    {
        return Result.Failure(Code, Errors, Message);
    }
}
