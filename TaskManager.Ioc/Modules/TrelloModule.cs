using Manatee.Trello;
using Microsoft.Extensions.DependencyInjection;
using TaskManager.Common;
using TaskManager.Common.Tasks;
using TaskManager.Trello;

namespace TaskManager.Ioc.Modules
{
    public class TrelloModule : IServiceModule
    {
        private readonly string appKey;

        public TrelloModule(string appKey) => this.appKey = appKey;

        public void Load(IServiceCollection services)
        {
            services.AddSingleton(new AppKey {Key = appKey});
            services.AddScoped<ITrelloFactory, TrelloFactory>();
            services.AddScoped<ITaskHandler, TrelloTasksHandler>();
            services.AddScoped<IAuthorizationProvider, TrelloAuthorizationProvider>();
            services.AddScoped<ITextButtonMenuProvider, TrelloTasksHandler>();
        }
    }
}