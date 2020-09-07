using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using TaskManager.Bot.Model.Domain;

namespace TaskManager.Bot.Model.Session
{
    public class MongoDbSessionstorage : ISessionStorage
    {
        private readonly IMongoCollection<SessionMongoEntity> collection;

        private readonly ReplaceOptions replaceOptions = new ReplaceOptions
        {
            IsUpsert = true
        };

        public MongoDbSessionstorage(IMongoDatabase mongoDatabase) =>
            collection = mongoDatabase
                .GetCollection<SessionMongoEntity>("sessions");


        public void HandleCommandSession(Author author, int commandIndex, SessionStatus sessionStatus,
            ISessionMeta sessionMeta)
        {
            if (sessionStatus != SessionStatus.Expect)
                collection.FindOneAndDelete(GetFilter(author));
            else
                collection.ReplaceOne(
                    GetFilter(author),
                    new SessionMongoEntity
                    {
                        TelegramId = author.TelegramId,
                        CommandIndex = commandIndex,
                        ContinueFrom = sessionMeta.ContinueFrom
                    },
                    replaceOptions);
        }

        public bool TryGetUserSession(Author author, out ISession session)
        {
            var sessionMeta = collection.FindSync(GetFilter(author)).FirstOrDefault();

            if (sessionMeta == null)
            {
                session = null;
                return false;
            }

            session = new Session(
                sessionMeta.CommandIndex,
                new SessionMeta
                {
                    ContinueFrom = sessionMeta.ContinueFrom
                });

            return true;
        }

        private static FilterDefinition<SessionMongoEntity> GetFilter(Author author)
            => Builders<SessionMongoEntity>.Filter.Eq("_id", author.TelegramId);


        private class SessionMongoEntity
        {
            [BsonRequired] [BsonElement("_id")] public long TelegramId { get; set; }
            [BsonElement("command_index")] public int CommandIndex { get; set; }
            [BsonElement("continue_from")] public int ContinueFrom { get; set; }
        }
    }
}