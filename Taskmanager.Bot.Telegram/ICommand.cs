using TaskManager.Bot.Telegram.Model;

namespace TaskManager.Bot.Telegram
{
    public interface ICommand
    {
        bool IsPublicCommand { get; }
        string CommandTrigger { get; }
        ICommandResponse StartCommand(ICommandInfo commandInfo);
    }
}