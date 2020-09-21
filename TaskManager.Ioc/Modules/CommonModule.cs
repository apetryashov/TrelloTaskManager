using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TaskManager.Common;
using TelegramBot.Core.Domain;

namespace TaskManager.Ioc.Modules
{
    public class CommonModule : IServiceModule
    {
        public void Load(IServiceCollection services)
        {
            services.AddScoped<IUserItemsStorage<Author>>(provider =>
                new MongoUserItemsStorage<Author>(
                    provider.GetService<IMongoDatabase>(),
                    "author")
            );
        }
    }
}