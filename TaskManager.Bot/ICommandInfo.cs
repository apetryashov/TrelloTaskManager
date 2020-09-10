using TaskManager.Common.Domain;

namespace TaskManager.Bot
{
    public interface ICommandInfo
    {
        Author Author { get; }
        string Command { get; }
    }
}