using Domain.Common.Exceptions;

namespace Application.Common.Exceptions;

public class JamaaAgentForbiddenAccessException : BaseJamaaAgentException
{
    public JamaaAgentForbiddenAccessException() : base("Hub_Lack_Access_Error","entity has no access to this resource")
    { 
    }
}
