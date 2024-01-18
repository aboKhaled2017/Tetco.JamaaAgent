using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace API.CrossCuttings.OpenAPI
{
    public static class SwaggerOpenAPI
    {
        public static void AddAgentSwaggerAPIs(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => { ConfigureSwagger(c); });
        }
        private static void ConfigureSwagger(SwaggerGenOptions options)
        {
            var securityScheme = new OpenApiSecurityScheme()
            {
                Name = "X-Agent-API-KEY",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyAuthentication",
                In = ParameterLocation.Header | ParameterLocation.Query,
                Description = "API Key based security"
            };

            var securityReq = new OpenApiSecurityRequirement()
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey",
                        },
                        In = ParameterLocation.Header| ParameterLocation.Query,
                    },
                    new string[] {}
                }
            };



            var info = new OpenApiInfo()
            {
                Version = "v1",
                Title = "Agent API",
            };

            options.SwaggerDoc("v1", info);
            options.AddSecurityDefinition("ApiKey", securityScheme);
            options.AddSecurityRequirement(securityReq);
        }
    }
}
