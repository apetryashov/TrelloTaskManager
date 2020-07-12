using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Ioc
{
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModules(this IServiceCollection services,
            IEnumerable<IServiceModule> modules)
        {
            foreach (var module in modules)
            {
                module.Load(services);
            }

            return services;
        }

        public static IServiceCollection AddModule<TModule>(this IServiceCollection services)
            where TModule : IServiceModule, new()
        {
            new TModule().Load(services);

            return services;
        }

        public static IServiceCollection AddModule<TModule>(this IServiceCollection services, TModule module)
            where TModule : IServiceModule
        {
            module.Load(services);

            return services;
        }
    }
}