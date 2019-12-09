using TaskManager.Bot.Model;

namespace TaskManager.Bot
{
    public interface IRequestHandler
    {
        IResponse GetResponse(IRequest request);
    }
}