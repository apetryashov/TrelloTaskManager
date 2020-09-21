using TelegramBot.Core.Model;

namespace TelegramBot.Core.Commands
{
    public interface ICommand
    {
        IResponse StartCommand(ICommandInfo commandInfo);
    }
}