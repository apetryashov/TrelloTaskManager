using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TaskManager.Bot.WebHook.Services
{
    //TODO: непонятный интерфейс
    public interface IUpdateService
    {
        Task EchoAsync(Update update);
    }
}