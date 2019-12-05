using TaskManager.Bot.Telegram.Model.Domain;

namespace TaskManager.Bot.Telegram.Commands
{
    public interface IAuthorizationStorage
    {
        bool IsAuthorizedUser(Author author);
        string GetUserToken(Author author);
        void SetUserToken(Author author, string token);
    }
}