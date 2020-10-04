using System.Collections.Generic;
using System.Linq;
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

            return authorWithItems == null ? default : ExtractItemOrDefault(authorWithItems);
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
                    GetItemFilter()
                );

            return collection.Find(filter).FirstOrDefault() != null;
        }

        public IEnumerable<(long id, TItem item)> GetAllItems() => collection.Find(GetItemFilter())
            .ToEnumerable()
            .Select(authorWithItems => (
                authorWithItems.Id,
                ExtractItemOrDefault(authorWithItems)
            ));

        private TItem ExtractItemOrDefault(AuthorWithItems authorWithItems)
        {
            if (authorWithItems.Items.TryGetValue(itemFieldName, out var item))
                return (TItem) item;

            return default;
        }

        private static FilterDefinition<AuthorWithItems> GetFilter(long id) =>
            Builders<AuthorWithItems>.Filter.Eq("_id", id);

        private FilterDefinition<AuthorWithItems> GetItemFilter() =>
            Builders<AuthorWithItems>.Filter.Exists($"Items.{itemFieldName}");

        private class AuthorWithItems
        {
            [BsonElement("_id")] public long Id { get; set; }

            public Dictionary<string, object> Items { get; set; } = new Dictionary<string, object>();
        }
    }
}