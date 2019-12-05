using TaskManager.Bot.Telegram.Model.Domain;

namespace TaskManager.Bot.Telegram.Model
{
    public class BaseRequest : IRequest
    {
        public BaseRequest(Author author, string message, long chatId)
        {
            Author = author;
            Command = message;
            ChatId = chatId;
        }

        public long ChatId { get; }
        public Author Author { get; }
        public string Command { get; }
    }
}