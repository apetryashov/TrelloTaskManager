using System.Threading.Tasks;
using TelegramBot.Core.Model;

namespace TelegramBot.Core.Commands
{
    public interface ICommand
    {
        Task<IResponse> StartCommand(ICommandInfo commandInfo);
    }
}