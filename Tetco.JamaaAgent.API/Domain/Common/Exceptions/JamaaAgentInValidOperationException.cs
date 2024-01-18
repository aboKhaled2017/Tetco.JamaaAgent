namespace Domain.Common.Exceptions;
public class JamaaAgentInValidOperationException : BaseJamaaAgentException
{
    public JamaaAgentInValidOperationException(string errorMessage = "not valid business operation")
        : base("Hub_Business_Validation_Error", errorMessage)
    {
    }
}
