using API.CrossCuttings.Authorization;
using API.CrossCuttings.MiddleWares;
using Microsoft.AspNetCore.Mvc;


namespace API;

public static class DependencyInjection
{
    public static IServiceCollection AddAPIServices(this IServiceCollection services)
    {

        services.AddSingleton<IAgentApiKeyValidator, AgentApiKeyValidator>();
        services.AddScoped<AgentApiKeyAuthorizationFilter>();
        services.AddSingleton<RequestResponseLoggingMiddleware>();
        services.AddTransient<JamaaAgentExceptionMiddleWare>();


        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();



        return services;
    }
}

