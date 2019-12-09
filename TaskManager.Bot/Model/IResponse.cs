using TaskManager.Bot.Model.Session;

namespace TaskManager.Bot.Model
{
    public interface IResponse
    {
        SessionStatus SessionStatus { get; }
    }
}