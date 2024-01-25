using Application.Common.Interfaces;
using Domain.Common.Settings;
using Domain.Enums;
using Infrastructure.Respos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddJamaaAgentAuthorization(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration, GeneralSetting generalSetting)
    {
        services.AddJamaaAgentAuthorization();
        services.AddSingleton(TimeProvider.System);
        void RegisterRequiredProvider(Type serviceType)
        {
            services.AddScoped(typeof(IStudentQuery), serviceType);
        }
        RegisterRequiredProvider((Provider)generalSetting.StudentProviderId switch
        {
            Provider.SQL => typeof(StudentQueryBySQLDbprovider),
            Provider.MySQL => typeof(StudentQueryByMySQLDbProvider),
            Provider.Oracle => typeof(StudentQueryByOracleDbprovider),
            // Add other cases as necessary
            _ => throw new InvalidOperationException($"Unsupported provider type: {generalSetting.StudentProviderId}")
        });

        return services;
    }
}
