using TaskManager.Bot.Model;

namespace TaskManager.Bot
{
    public interface ICommand
    {
        IResponse StartCommand(ICommandInfo commandInfo);
    }
}