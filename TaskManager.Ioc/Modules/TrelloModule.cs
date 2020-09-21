using Manatee.Trello;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TaskManager.Common;
using TaskManager.Common.Tasks;
using TaskManager.Trello;

namespace TaskManager.Ioc.Modules
{
    public class TrelloModule : IServiceModule
    {
        private readonly string appKey;
        private readonly string returnUrl;

        public TrelloModule(string appKey, string returnUrl)
        {
            this.appKey = appKey;
            this.returnUrl = returnUrl;
        }

        public void Load(IServiceCollection services)
        {
            services.AddSingleton(new AppKey {Key = appKey});
            services.AddSingleton(new ReturnUrl {Url = returnUrl});
            services.AddScoped<ITrelloFactory, TrelloFactory>();

            services.AddScoped<IUserItemsStorage<TrelloApiToken>>(provider =>
                new MongoUserItemsStorage<TrelloApiToken>(
                    provider.GetService<IMongoDatabase>(),
                    "trello-token")
            );

            services.AddScoped<ITrelloFactory, TrelloFactory>();
            services.AddScoped<ITaskHandler, TrelloTasksHandler>();
            services.AddScoped<IAuthorizationProvider, TrelloAuthorizationProvider>();
            services.AddScoped<ITextButtonMenuProvider, TrelloTasksHandler>();
        }
    }
}