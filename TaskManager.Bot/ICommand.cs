using TaskManager.Bot.Model;

namespace TaskManager.Bot
{
    public interface ICommand
    {
        bool IsPublicCommand { get; }
        string CommandTrigger { get; }
        ICommandResponse StartCommand(ICommandInfo commandInfo);
    }
}