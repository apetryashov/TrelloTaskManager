using TaskManaget.Bot.Model;

namespace TaskManaget.Bot
{
    public interface IRequestHandler
    {
        IResponse GetResponse(IRequest request);
    }
}