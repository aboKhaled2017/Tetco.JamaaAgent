using Application.Common.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddJamaaAgentAuthorization(this IServiceCollection services)
    {
  
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddJamaaAgentAuthorization();
        
        services.AddSingleton(TimeProvider.System);
  

        return services;
    }
}
