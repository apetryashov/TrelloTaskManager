using System.Threading.Tasks;
using TelegramBot.Core.Model;

namespace TaskManager.Bot
{
    public interface IRequestHandler
    {
        Task<IResponse> GetResponse(IRequest request);
    }
}