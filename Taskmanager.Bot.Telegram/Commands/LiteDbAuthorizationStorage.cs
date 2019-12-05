using System;
using LiteDB;
using TaskManager.Bot.Telegram.Model.Domain;

namespace TaskManager.Bot.Telegram.Commands
{
    public class LiteDbAuthorizationStorage : IAuthorizationStorage
    {
        private readonly LiteCollection<AuthorizationInfo> collection;

        public LiteDbAuthorizationStorage(LiteDatabase liteDatabase)
        {
            collection = liteDatabase.GetCollection<AuthorizationInfo>();
        }
        
        public bool IsAuthorizedUser(Author author)
        {
            return collection.Exists(x => x.TelegramId == author.TelegramId);
        }

        public string GetUserToken(Author author)
        {
            var authorizationInfo = collection.FindOne(x => x.TelegramId == author.TelegramId);

            if (authorizationInfo == null)
                throw new ArgumentException("can't find user authorization info");

            return authorizationInfo.UserToken;
        }

        public void SetUserToken(Author author, string token)
        {
            var authorizationInfo = new AuthorizationInfo
            {
                TelegramId = author.TelegramId,
                UserToken = token
            };

            if (IsAuthorizedUser(author))
            {
                collection.Update(authorizationInfo);
                return;
            }

            collection.Insert(authorizationInfo);
        }

        private class AuthorizationInfo
        {
            public long TelegramId { get; set; }
            public string UserToken { get; set; }
        }
    }
}