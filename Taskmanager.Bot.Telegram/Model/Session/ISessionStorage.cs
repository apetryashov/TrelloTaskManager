using TaskManager.Bot.Telegram.Model.Domain;

namespace TaskManager.Bot.Telegram.Model.Session
{
    public interface ISessionStorage
    {
        void HandleCommandSession(Author author, int commandIndex, SessionStatus sessionStatus, ISessionMeta sessionMeta);
        bool TryGetUserSession(Author author, out ISession session);
    }
}