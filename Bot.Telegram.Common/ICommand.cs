using Bot.Telegram.Common.Model;

namespace Bot.Telegram.Common
{
    public interface ICommand
    {
        bool IsPublicCommand { get; }
        string CommandTrigger { get; }
        ICommandResponse StartCommand(ICommandInfo commandInfo);
    }
}