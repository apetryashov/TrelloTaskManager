using Bot.Telegram.Common.Model;

namespace Bot.Telegram.Common
{
    public interface ICommand
    {
        string CommandTrigger { get; }
        ICommandResponse StartCommand(ICommandInfo commandInfo);
    }
}