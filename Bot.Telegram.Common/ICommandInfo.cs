using Bot.Telegram.Common.Model.Domain;
using Bot.Telegram.Common.Model.Session;
using JetBrains.Annotations;

namespace Bot.Telegram.Common
{
    public interface ICommandInfo
    {
        Author Author { get; }
        string Command { get; }
        [CanBeNull] ISessionMeta SessionMeta { get; }
    }
}