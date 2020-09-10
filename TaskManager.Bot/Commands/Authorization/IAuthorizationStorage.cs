using TaskManager.Common.Domain;

namespace TaskManager.Bot.Commands.Authorization
{
    public interface IAuthorizationStorage
    {
        bool TryGetUserToken(Author author, out string token);
        void SetUserToken(Author author, string token);
    }
}