using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace API.CrossCuttings.Authorization
{
    public sealed class AgentApiKeyAuthorizationFilter : IAuthorizationFilter
    {
        private const string ApiKeyHeaderName = "X-Agent-API-Key";
        private readonly IAgentApiKeyValidator _apiKeyValidator;

        public AgentApiKeyAuthorizationFilter(IAgentApiKeyValidator apiKeyValidator)
        {
            _apiKeyValidator = apiKeyValidator;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];
            if (string.IsNullOrEmpty(apiKey))
                apiKey = context.HttpContext.Request.Query[ApiKeyHeaderName].FirstOrDefault()!;

            if (string.IsNullOrEmpty(apiKey) || !_apiKeyValidator.IsValid(apiKey))
                context.Result = new UnauthorizedResult();
        }
    }
}
