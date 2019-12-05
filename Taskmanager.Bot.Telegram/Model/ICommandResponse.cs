using TaskManager.Bot.Telegram.Model.Session;

namespace TaskManager.Bot.Telegram.Model
{
    public interface ICommandResponse
    {
        IResponse Response { get; }
        ISessionMeta SessionMeta { get; }
    }
}    