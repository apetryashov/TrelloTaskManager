using TaskManaget.Bot.Model.Session;

namespace TaskManaget.Bot.Model
{
    public interface IResponse
    {
        SessionStatus SessionStatus { get; }
    }
}