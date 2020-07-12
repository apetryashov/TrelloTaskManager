using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace TaskManager.Bot.WebHook.Services
{
    public interface IUpdateService
    {
        Task EchoAsync(Update update);
    }
}