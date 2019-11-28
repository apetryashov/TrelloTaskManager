using System;
using System.Collections.Generic;
using Bot.Telegram.Common.Model.Domain;

namespace Bot.Telegram.Common.Model.Session
{
    public class InMemorySessionStorage : ISessionStorage
    {
        private readonly Dictionary<long, ISession> usersActiveSessions = new Dictionary<long, ISession>();

        public void HandleCommandSession(Author author, int commandIndex, ICommandSession session)
        {
            var status = session.SessionStatus;
            if (status == SessionStatus.Expect)
            {
                if (session.ContinueIndex.HasValue)
                {
                    usersActiveSessions[author.TelegramId] = new Session(commandIndex, session.ContinueIndex.Value);
                }
                else
                {
                    throw new Exception(); // fix it
                }
            }
            else if (status == SessionStatus.Close)
            {
                usersActiveSessions.Remove(author.TelegramId);
            }
        }

        public bool TryGetUserSession(Author author, out ISession session)
        {
            return usersActiveSessions.TryGetValue(author.TelegramId, out session);
        }
    }
}