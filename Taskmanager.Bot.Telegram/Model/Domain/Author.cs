using JetBrains.Annotations;

namespace Bot.Telegram.Common.Model.Domain
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

        public long TelegramId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string UserName { get; private set; }
        
        [CanBeNull] public string UserToken { get; set; }
    }
}