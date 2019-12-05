using TaskManager.Bot.Telegram.Model;

namespace TaskManager.Bot.Telegram
{
    public interface IRequestHandler
    {
        IResponse GetResponse(IRequest request);
    }
}