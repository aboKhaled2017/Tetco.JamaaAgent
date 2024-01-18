using Microsoft.AspNetCore.Mvc;

namespace API.CrossCuttings.Authorization
{
    public class MustHaveAgentApiKeyAttribute : ServiceFilterAttribute
    {
        public MustHaveAgentApiKeyAttribute() : base(typeof(AgentApiKeyAuthorizationFilter))
        {
        }
    }
}
