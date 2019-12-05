using TaskManager.Bot.Telegram.Model.Session;

namespace TaskManager.Bot.Telegram.Model
{
    public interface IResponse
    {
        SessionStatus SessionStatus { get; }
    }
}