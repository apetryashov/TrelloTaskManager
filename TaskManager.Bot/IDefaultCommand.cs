using TaskManager.Bot.Model;

namespace TaskManager.Bot
{
    public interface IDefaultCommand
    {
        IResponse StartCommand(ICommandInfo commandInfo);
    }
}