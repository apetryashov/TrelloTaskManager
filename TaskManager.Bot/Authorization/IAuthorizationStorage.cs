using TaskManager.Bot.Model.Domain;

namespace TaskManager.Bot.Authorization
{
    public interface IAuthorizationStorage
    {
        bool TryGetUserToken(Author author, out string token);
        void SetUserToken(Author author, string token);
    }
}