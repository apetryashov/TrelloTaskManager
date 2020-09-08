using TaskManager.Bot.Model.Domain;

namespace TaskManager.Bot
{
    public interface ICommandInfo
    {
        Author Author { get; }
        string Command { get; }
    }
}