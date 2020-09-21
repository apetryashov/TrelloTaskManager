using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using TaskManager.Bot.Commands.Authorization;
using TaskManager.Common;

namespace TaskManager.Ioc.Modules
{
    public class MongoDbAuthorizationStorageModule : IServiceModule
    {
        private readonly MongoConnectionProperties connectionProperties;

        public MongoDbAuthorizationStorageModule(MongoConnectionProperties connectionProperties)
            => this.connectionProperties = connectionProperties;

        public void Load(IServiceCollection services)
        {
            var url = MongoUrl.Create(connectionProperties.GetConnectionString());
            var mongoDb = new MongoClient(url)
                .GetDatabase(url.DatabaseName);

            services.AddSingleton(mongoDb);
        }
    }
}