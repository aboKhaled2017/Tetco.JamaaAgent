using Application.Common.Interfaces;
using Domain.Common.Settings;
using Domain.Enums;
using Infrastructure.Respos;
using Infrastructure.Respos.Dynamic;
using Infrastructure.Respos.Students;
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
        RegisterRequiredProvider((DBProvider)generalSetting.StudentProviderId switch
        {
            DBProvider.SQL => typeof(StudentQueryBySQLDbprovider),
            DBProvider.MySQL => typeof(StudentQueryByMySQLDbProvider),
            DBProvider.Oracle => typeof(StudentQueryByOracleDbprovider),
            // Add other cases as necessary
            _ => throw new InvalidOperationException($"Unsupported provider type: {generalSetting.StudentProviderId}")
        });

        services.AddScoped<IDefineProviderManager, DefineProviderManager>();
        services.AddScoped<ICreateDatabaseFactory, CreateDatabaseFactory>();
        services.AddScoped<IDynamicQuery, DynamicQueryByMYSQLDbprovider>();
        services.AddScoped<IDynamicQuery, DynamicQueryBySQLDbprovider>();
        services.AddScoped<IDynamicQuery, DynamicQueryByORACLEDbprovider>();
       
        

        return services;
    }
}
