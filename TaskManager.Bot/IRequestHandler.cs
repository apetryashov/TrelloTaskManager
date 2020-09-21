using TelegramBot.Core.Model;

namespace TaskManager.Bot
{
    public interface IRequestHandler
    {
        IResponse GetResponse(IRequest request);
    }
}