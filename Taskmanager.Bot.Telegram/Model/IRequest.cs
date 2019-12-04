using Bot.Telegram.Common.Model.Domain;

namespace Bot.Telegram.Common.Model
{
    public interface IRequest
    {
        Author Author { get; }
        string Command { get; }
    }
}