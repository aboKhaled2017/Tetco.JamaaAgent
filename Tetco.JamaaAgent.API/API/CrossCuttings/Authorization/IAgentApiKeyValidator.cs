using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using System.Text;

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

        //public bool IsValid(string apiKey)
        //{
        //    //this could be check from database instead of appsettings configuration or any data source
        //    return _configuration.GetValue<string>("Auth:AgentKey") == apiKey;
        //}

        public  bool IsValid(string apiKey)
        {
            var storedHashedPassword = _configuration.GetValue<string>("Auth:AgentKey");

            // Hash the entered password using the same algorithm as during the initial hashing
            string hashedEnteredPassword = HashPassword(apiKey);

            // Compare the entered hashed password with the stored hashed password
            return string.Equals(hashedEnteredPassword, storedHashedPassword, StringComparison.Ordinal);
        }

        public static string HashPassword(string apiKey)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(apiKey));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}
