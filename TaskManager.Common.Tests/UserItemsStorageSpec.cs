using FluentAssertions;
using MongoDB.Driver;
using NUnit.Framework;

namespace TaskManager.Common.Tests
{
    [TestFixture]
    public class UserItemsStorageSpec
    {
        private MongoUserItemsStorage<TestItem> storage;
        private const string CollectionName = "test-entity";
        private IMongoDatabase mongoDatabase;
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var connectionProperties = new MongoConnectionProperties
            {
                
            };
            var url = MongoUrl.Create(connectionProperties.GetConnectionString());
            var mongoClient = new MongoClient(url);
            mongoClient.DropDatabase(url.DatabaseName);
            mongoDatabase = mongoClient
                .GetDatabase(url.DatabaseName);

            storage = new MongoUserItemsStorage<TestItem>(mongoDatabase, CollectionName);
        }

        [SetUp]
        public void SetUp() => mongoDatabase.DropCollection(CollectionName);

        [Test]
        public void Should_set_and_get_item()
        {
            const int authorId = 1;
            var item = GetTestItem(authorId);

            storage.Set(authorId, item);
            var itemFromStorage = storage.Get(authorId);

            itemFromStorage.Should().BeEquivalentTo(item);
        }

        [Test]
        public void Should_update_item_after_reinsert()
        {
            const int authorId = 1;
            var item = GetTestItem(authorId);;
            var newItem = GetTestItem(authorId);
            newItem.TestLongField = 321;

            storage.Set(authorId, item);
            storage.Set(authorId, newItem);
            var itemFromStorage = storage.Get(authorId);

            itemFromStorage.Should().BeEquivalentTo(newItem);
        }

        [Test]
        public void Should_return_null_when_item_was_not_found()
        {
            const int authorId = 1;
            
            var itemFromStorage = storage.Get(authorId);
            
            itemFromStorage.Should().BeNull();
        }

        [Test]
        public void Should_set_null_after_delete()
        {
            const int authorId = 1;
            var item = GetTestItem(authorId);
            
            storage.Set(authorId, item);
            storage.Delete(authorId);
            var itemFromStorage = storage.Get(authorId);
            
            itemFromStorage.Should().BeNull();
        }

        [Test]
        public void Should_return_false_when_item_not_fount()
        {
            const int authorId = 1;
            
            storage.Has(authorId).Should().BeFalse();
        }
        
        [Test]
        public void Should_return_false()
        {
            const int authorId = 1;
            var item = GetTestItem(authorId);
            
            storage.Set(authorId, item);
            storage.Delete(authorId);
            
            storage.Has(authorId).Should().BeFalse();
        }

        [Test]
        public void Should_return_true_when_item_exists()
        {
            const int authorId = 1;
            var item = GetTestItem(authorId);
            
            storage.Set(authorId, item);
            
            storage.Has(authorId).Should().BeTrue();
        }

        private static TestItem GetTestItem(long id) => new TestItem
        {
            TestLongField = id,
            TestStringField = $"field_for_user_with_id_{id}"
        };
        
        private class TestItem
        {
            public string TestStringField { get; set; }
            public long TestLongField { get; set; }
        }
    }
}