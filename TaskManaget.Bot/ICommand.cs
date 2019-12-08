using TaskManaget.Bot.Model;

namespace TaskManaget.Bot
{
    public interface ICommand
    {
        bool IsPublicCommand { get; }
        string CommandTrigger { get; }
        ICommandResponse StartCommand(ICommandInfo commandInfo);
    }
}