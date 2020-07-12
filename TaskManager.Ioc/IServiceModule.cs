using Microsoft.Extensions.DependencyInjection;

namespace TaskManager.Ioc
{
    public interface IServiceModule
    {
        public void Load(IServiceCollection serviceCollection);
    }
}