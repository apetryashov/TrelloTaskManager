namespace Bot.Telegram.Common.Model.Session
{
    public interface ISession
    {
        int CommandId { get; }
        ISessionMeta SessionMeta { get; }
    }
}