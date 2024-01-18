using Domain.Common.Exceptions;
using FluentValidation.Results;

namespace Application.Common.Exceptions;

public class JamaaAgentValidationException : BaseJamaaAgentException
{
    public JamaaAgentValidationException()
        : base("Hub_Not_Valid_Request_Error", "One or more validation failures have occurred.")
    {

    }

    public JamaaAgentValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
            .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
    }
}
