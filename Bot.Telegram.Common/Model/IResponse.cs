using Bot.Telegram.Common.Model.Session;

namespace Bot.Telegram.Common.Model
{
    public interface IResponse
    {
        SessionStatus SessionStatus { get; }
    }
}