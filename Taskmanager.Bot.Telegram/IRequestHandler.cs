using Bot.Telegram.Common.Model;

namespace Bot.Telegram.Common
{
    public interface IRequestHandler
    {
        IResponse GetResponse(IRequest request);
    }
}