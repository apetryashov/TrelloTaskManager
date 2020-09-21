using JetBrains.Annotations;

namespace TelegramBot.Core.Domain
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

        public long TelegramId { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}
        public string UserName { get; set;}
    }
}