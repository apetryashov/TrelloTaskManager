using System.Collections.Generic;
using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;

namespace Bot.Telegram.Common.Model
{
    public class InMemorySessionStorage : ISessionStorage
    {
        private readonly Dictionary<long, ISession> usersActiveSessions = new Dictionary<long, ISession>();

        public void AddUserSession(Author author, ISession session)
        {
            usersActiveSessions[author.TelegramId] = session;
        }

        public void KillUserSession(Author author)
        {
            usersActiveSessions.Remove(author.TelegramId);
        }

        public bool TryGetUserSession(Author author, out ISession session)
        {
            return usersActiveSessions.TryGetValue(author.TelegramId, out session);
        }
    }

    public interface ISessionStorage
    {
        void AddUserSession(Author author, ISession session);
        void KillUserSession(Author author);
        bool TryGetUserSession(Author author, out ISession session);
    }
}