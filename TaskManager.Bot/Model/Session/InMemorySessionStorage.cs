using System.Collections.Generic;
using TaskManager.Bot.Model.Domain;

namespace TaskManager.Bot.Model.Session
{
    public class InMemorySessionStorage : ISessionStorage
    {
        private readonly Dictionary<long, ISession> usersActiveSessions = new Dictionary<long, ISession>();

        public void HandleCommandSession(Author author, int commandIndex, SessionStatus sessionStatus,
            ISessionMeta sessionMeta)
        {
            if (sessionStatus == SessionStatus.Expect)
                usersActiveSessions[author.TelegramId] = new Session(commandIndex, sessionMeta);
            else
                usersActiveSessions.Remove(author.TelegramId);
        }

        public bool TryGetUserSession(Author author, out ISession session)
            => usersActiveSessions.TryGetValue(author.TelegramId, out session);
    }
}