using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Infrastructure.Utilities
{
    public static class ModelBuilderUtilities
    {
        public static void RegisterConfigurationsFromNameSpace(this ModelBuilder builder,Assembly assembly, string targetNamespace)
        {
            var entityTypes = assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
            .Any(interfaceType => interfaceType.IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)) && t.Namespace == targetNamespace);
            foreach (var entityType in entityTypes)
            {
               // _logger.LogInformation($"Applying configuration for entity type: {entityType.FullName}");
                dynamic instance = Activator.CreateInstance(entityType);
                builder.ApplyConfiguration(instance);
            }
        }
    }
}
