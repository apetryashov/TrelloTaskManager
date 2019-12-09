using JetBrains.Annotations;

namespace TaskManager.Bot.Model.Domain
{
    public class Author
    {
        public Author(long telegramId, string firstName, string lastName, string username)
        {
            TelegramId = telegramId;
            FirstName = firstName;
            LastName = lastName;
            UserName = username;
        }

        public long TelegramId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string UserName { get; }

        [CanBeNull] public string UserToken { get; set; }
    }
}