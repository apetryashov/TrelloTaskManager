using Bot.Telegram.Common.Model.Session;

namespace Bot.Telegram.Common.Model
{
    public interface ICommandResponse
    {
        IResponse Response { get; }
        ICommandSession Session { get; }
    }
}