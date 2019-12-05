namespace TaskManager.Bot.Telegram.Model.Session
{
    public interface ISession
    {
        int CommandId { get; }
        ISessionMeta SessionMeta { get; }
    }
}