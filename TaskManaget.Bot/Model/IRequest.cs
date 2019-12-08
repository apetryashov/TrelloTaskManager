using TaskManaget.Bot.Model.Domain;

namespace TaskManaget.Bot.Model
{
    public interface IRequest
    {
        Author Author { get; }
        string Command { get; }
    }
}