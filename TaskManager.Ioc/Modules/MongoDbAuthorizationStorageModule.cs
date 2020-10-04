using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using TaskManager.Common;
using TaskManager.Trello;

namespace TaskManager.Ioc.Modules
{
    public class MongoDbAuthorizationStorageModule : IServiceModule
    {
        private readonly MongoConnectionProperties connectionProperties;

        public MongoDbAuthorizationStorageModule(MongoConnectionProperties connectionProperties)
            => this.connectionProperties = connectionProperties;

        public void Load(IServiceCollection services)
        {
            BsonClassMap.RegisterClassMap<TrelloApiToken>();
            var url = MongoUrl.Create(connectionProperties.GetConnectionString());
            var mongoDb = new MongoClient(url)
                .GetDatabase(url.DatabaseName);

            services.AddSingleton(mongoDb);
        }
    }
}