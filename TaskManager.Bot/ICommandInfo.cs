using JetBrains.Annotations;
using TaskManager.Bot.Model.Domain;
using TaskManager.Bot.Model.Session;

namespace TaskManager.Bot
{
    public interface ICommandInfo
    {
        Author Author { get; }
        string Command { get; }
        [CanBeNull] ISessionMeta SessionMeta { get; }
    }
}