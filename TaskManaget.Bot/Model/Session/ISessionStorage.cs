using TaskManaget.Bot.Model.Domain;

namespace TaskManaget.Bot.Model.Session
{
    public interface ISessionStorage
    {
        void HandleCommandSession(Author author, int commandIndex, SessionStatus sessionStatus, ISessionMeta sessionMeta);
        bool TryGetUserSession(Author author, out ISession session);
    }
}