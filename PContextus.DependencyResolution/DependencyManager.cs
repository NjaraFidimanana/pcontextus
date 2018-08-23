namespace PContextus.DependencyResolution
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;

    using PG.Services.Console.DependencyResolution.Registries;

    public static class DependencyManager
    {
        public static void BuildDependencies(IServiceCollection services)
        {
            var type = typeof(IDependency);

            var typeInfo = type.GetTypeInfo();

            var instances = typeInfo
                .Assembly
                .GetExportedTypes()
                .Where(t => type.IsAssignableFrom(t) && t.GetTypeInfo().IsClass)
                .Select(t => Activator.CreateInstance(t) as IDependency)
                .ToList();

            instances.ForEach(instance => instance.Register(services));
        }
    }
}
