using TaskManaget.Bot.Model.Session;

namespace TaskManaget.Bot.Model
{
    public interface ICommandResponse
    {
        IResponse Response { get; }
        ISessionMeta SessionMeta { get; }
    }
}    