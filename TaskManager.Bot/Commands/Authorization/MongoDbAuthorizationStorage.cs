using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using TaskManager.Common.Domain;

namespace TaskManager.Bot.Commands.Authorization
{
    public class MongoDbAuthorizationStorage : IAuthorizationStorage
    {
        private readonly IMongoCollection<AuthorizationInfo> collection;

        public MongoDbAuthorizationStorage(IMongoDatabase mongoDatabase) =>
            collection = mongoDatabase
                .GetCollection<AuthorizationInfo>("authorization");

        public bool TryGetUserToken(Author author, out string token)
        {
            var filter = Builders<AuthorizationInfo>.Filter.Eq("_id", author.TelegramId);
            var authorizationInfo = collection.FindSync(filter).FirstOrDefault();

            if (authorizationInfo == null)
            {
                token = null;
                return false;
            }

            token = authorizationInfo.Token;
            return true;
        }

        public void SetUserToken(Author author, string token) =>
            collection.InsertOne(new AuthorizationInfo
            {
                TelegramId = author.TelegramId,
                Token = token
            });

        private class AuthorizationInfo
        {
            [BsonRequired] [BsonElement("_id")] public long TelegramId { get; set; }
            [BsonElement("token")] public string Token { get; set; }
        }
    }
}