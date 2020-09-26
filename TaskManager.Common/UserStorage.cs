using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace TaskManager.Common
{
    public class MongoUserItemsStorage<TItem> : IUserItemsStorage<TItem>
    {
        private readonly IMongoCollection<AuthorWithItems> collection;
        private readonly string itemFieldName;

        public MongoUserItemsStorage(IMongoDatabase mongoDatabase, string itemFieldName)
        {
            this.itemFieldName = itemFieldName;
            collection = mongoDatabase
                .GetCollection<AuthorWithItems>("users-info");

            // var pack = new ConventionPack {new CamelCaseElementNameConvention()};
            //
            // ConventionRegistry.Register(
            //     "all_items_convention_registry",
            //     pack,
            //     x => true);
        }

        public void Set(long id, TItem item)
        {
            var filter = GetFilter(id);
            var update = Builders<AuthorWithItems>.Update
                .Set(x => x.Items[itemFieldName], item);

            collection.UpdateOne(filter, update, new UpdateOptions
            {
                IsUpsert = true
            });
        }

        public TItem Get(long id)
        {
            var filter = GetFilter(id);
            var authorWithItems = collection.Find(filter).FirstOrDefault();

            if (authorWithItems == null)
                return default;

            if (authorWithItems.Items.TryGetValue(itemFieldName, out var item))
                return (TItem) item;

            return default;
        }

        public void Delete(long id)
        {
            var filter = GetFilter(id);
            var unset = Builders<AuthorWithItems>.Update
                .Unset($"Items.{itemFieldName}");

            collection.UpdateOne(filter, unset);
        }

        public bool Has(long id)
        {
            var filter = Builders<AuthorWithItems>.Filter
                .And(
                    GetFilter(id),
                    Builders<AuthorWithItems>.Filter.Exists($"Items.{itemFieldName}")
                );

            return collection.Find(filter).FirstOrDefault() != null;
        }

        private static FilterDefinition<AuthorWithItems> GetFilter(long id) =>
            Builders<AuthorWithItems>.Filter.Eq("_id", id);

        private class AuthorWithItems
        {
            [BsonElement("_id")] public long Id { get; set; }

            public Dictionary<string, object> Items { get; } = new Dictionary<string, object>();
        }
    }
}