using JetBrains.Annotations;
using TaskManaget.Bot.Model.Domain;
using TaskManaget.Bot.Model.Session;

namespace TaskManaget.Bot
{
    public interface ICommandInfo
    {
        Author Author { get; }
        string Command { get; }
        [CanBeNull] ISessionMeta SessionMeta { get; }
    }
}