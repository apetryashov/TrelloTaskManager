using TaskManager.Bot.Model.Domain;

namespace TaskManager.Bot.Authorization
{
    public interface IAuthorizationStorage
    {
        bool IsAuthorizedUser(Author author);
        string GetUserToken(Author author);
        void SetUserToken(Author author, string token);
    }
}