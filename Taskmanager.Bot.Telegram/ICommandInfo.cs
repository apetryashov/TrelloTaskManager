using JetBrains.Annotations;
using TaskManager.Bot.Telegram.Model.Domain;
using TaskManager.Bot.Telegram.Model.Session;

namespace TaskManager.Bot.Telegram
{
    public interface ICommandInfo
    {
        Author Author { get; }
        string Command { get; }
        [CanBeNull] ISessionMeta SessionMeta { get; }
    }
}