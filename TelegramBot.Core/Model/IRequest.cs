using TelegramBot.Core.Domain;

namespace TelegramBot.Core.Model
{
    public interface IRequest
    {
        Author Author { get; }
        string Command { get; }
    }
}