using Bot.Telegram.Common.Model.Domain;

namespace Bot.Telegram.Common.Model
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