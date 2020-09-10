using TaskManager.Common.Domain;

namespace TaskManager.Bot.Model
{
    public interface IRequest
    {
        Author Author { get; }
        string Command { get; }
    }
}