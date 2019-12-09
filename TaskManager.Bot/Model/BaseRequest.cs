using TaskManager.Bot.Model.Domain;

namespace TaskManager.Bot.Model
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