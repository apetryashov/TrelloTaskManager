using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Bot.Authorization;

namespace TaskManager.Ioc.Modules
{
    public class LiteDbStorageModule : IServiceModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddSingleton(new LiteDatabase("MyDb"));
            services.AddSingleton<IAuthorizationStorage, LiteDbAuthorizationStorage>();
        }
    }

    public class InMemoryAuthorizationStorageModule : IServiceModule
    {
        public void Load(IServiceCollection services)
            => services.AddSingleton<IAuthorizationStorage, InMemoryAuthorizationStorage>();
    }
}