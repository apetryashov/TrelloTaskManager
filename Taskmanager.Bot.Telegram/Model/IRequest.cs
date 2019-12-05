using TaskManager.Bot.Telegram.Model.Domain;

namespace TaskManager.Bot.Telegram.Model
{
    public interface IRequest
    {
        Author Author { get; }
        string Command { get; }
    }
}