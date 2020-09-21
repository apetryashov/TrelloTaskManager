using TelegramBot.Core.Domain;

namespace TelegramBot.Core.Commands
{
    public interface ICommandInfo
    {
        Author Author { get; }
        string Command { get; }
    }
}