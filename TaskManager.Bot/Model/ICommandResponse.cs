using TaskManager.Bot.Model.Session;

namespace TaskManager.Bot.Model
{
    public interface ICommandResponse
    {
        IResponse Response { get; }
        ISessionMeta SessionMeta { get; }
    }
}