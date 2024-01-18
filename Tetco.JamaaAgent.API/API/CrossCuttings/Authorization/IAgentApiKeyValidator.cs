namespace API.CrossCuttings.Authorization
{
    public interface IAgentApiKeyValidator
    {
        bool IsValid(string apiKey);
    }
    public class AgentApiKeyValidator : IAgentApiKeyValidator
    {
        private readonly IConfiguration _configuration;

        public AgentApiKeyValidator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool IsValid(string apiKey)
        {
            //this could be check from database instead of appsettings configuration or any data source
            return _configuration.GetValue<string>("Auth:AgentKey") == apiKey;
        }
    }
}
