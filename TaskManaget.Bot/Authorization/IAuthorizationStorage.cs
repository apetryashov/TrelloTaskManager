using TaskManaget.Bot.Model.Domain;

namespace TaskManaget.Bot.Authorization
{
    public interface IAuthorizationStorage
    {
        bool IsAuthorizedUser(Author author);
        string GetUserToken(Author author);
        void SetUserToken(Author author, string token);
    }
}